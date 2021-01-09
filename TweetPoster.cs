using System;
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

        public async Task PostNextTweet()
        {
            var dao = new DAOTweets();
            var tweet = dao.GetNextTweet();
            bool posted=false;

            if (tweet == null)
            {
                Console.WriteLine("There are no tweets yet");
                return;
            }
            try
            {
                if (tweet.type.Equals("post"))
                {
                    var image = await userClient.Upload.UploadTweetImageAsync(tweet.media);
                    await userClient.Tweets.PublishTweetAsync(new PublishTweetParameters(tweet.captions)
                    {
                        Medias = { image }
                    });
                }
                else if (tweet.type.Equals("retweet"))
                {
                    long tweetID = long.Parse(tweet.captions);
                    var retweet = await userClient.Tweets.PublishRetweetAsync(tweetID);
                    if (!retweet.Favorited) await retweet.FavoriteAsync();
                }
                posted = true;

            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e);
                posted = false;
            }
            dao.Delete(tweet);
            dao.UpdateStatus(tweet, posted);

            Console.WriteLine("Tweet deleted from db");
        }

    }
}
