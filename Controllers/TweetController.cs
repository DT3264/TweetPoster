using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TweetPoster.DB;

namespace TweetPoster.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TweetController : ControllerBase
    {
        [HttpGet]
        [Route("postTweet/{key}")]
        public async Task<string> PostTweet(string key)
        {
            if (key.Equals(Environment.GetEnvironmentVariable("SECRET")))
            {
                var dao = new DAOTweets();
                var tweet = dao.GetNextTweet();
                if (tweet == null)
                {
                    return "There are no tweets at the moment";
                }
                bool posted=false;
                //Begins some try catch mess
                if (tweet.type.Equals("post"))
                {
                    try
                    {
                        posted = await new TweetPoster().PostTweet(tweet);
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                }
                else if (tweet.type.Equals("retweet")) {
                    try
                    {
                        //Try retweeting
                        posted = await new TweetPoster().Retweet(tweet);
                    }
                    catch (Exception e)
                    {
                        //If cannot retweet, upload as image
                        Console.Error.WriteLine(e.Message);
                        try
                        {
                            //Appending the tweet id
                            tweet.captions = "Tweet ID: " + tweet.captions;
                            posted = await new TweetPoster().PostTweet(tweet);
                        }
                        catch (Exception innerE)
                        {
                            Console.Error.WriteLine(innerE.Message);
                        }
                    }
                }
                dao.Delete(tweet);
                dao.UpdateStatus(tweet, posted);
               
                return "Tweet posted";
            }
            else
            {
                return "That is not the secret word";
            }
        }
    }
}