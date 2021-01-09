using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                await new TweetPoster().PostNextTweet();
                return "Tweet posted";
            }
            else
            {
                return "That is not the secret word";
            }
        }
    }
}