﻿using System;
using System.Collections.Generic;
using System.Text;
using TweetPoster;
using Tweetinvi;
using System.Threading.Tasks;
using System.IO;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using TweetPoster.DB;

namespace TweetPoster
{
    class TweetPoster
    {
        TwitterClient userClient;
        public TweetPoster()
        {
            var consumerKey = Utils.GetEnvVar("CONSUMER_KEY");
            var consumerSecret = Utils.GetEnvVar("CONSUMER_SECRET");
            var accessToken = Utils.GetEnvVar("ACCESS_TOKEN");
            var accessSecret = Utils.GetEnvVar("ACCESS_SECRET");
            userClient = new TwitterClient(consumerKey, consumerSecret, accessToken, accessSecret);
        }

        public async Task<bool> Retweet(Tweet tweet)
        {
            try
            {
                long tweetID = long.Parse(tweet.captions);
                var retweet = await userClient.Tweets.PublishRetweetAsync(tweetID);
                if (!retweet.Favorited) await retweet.FavoriteAsync();

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                throw e;
            }
            return true;
        }

        public async Task<bool> PostTweet(Tweet tweet)
        {
            try
            {
                var text = ( tweet.type.Equals("post") ? "Source: " : "Source post/account deleted. Tweet ID: " ) + tweet.captions;
                var image = await userClient.Upload.UploadTweetImageAsync(tweet.media);
                await userClient.Tweets.PublishTweetAsync(
                    new PublishTweetParameters(text)
                    {
                        Medias = { image }
                    }
                );

            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e.Message);
                throw e;
            }
            return true;
        }

    }
}
