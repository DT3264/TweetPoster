using System;
using System.Collections.Generic;
using System.Text;

namespace TweetPoster
{
    public class Tweet
    {
        public int id { get; set; }
        public string captions { get; set; }
        public byte[] media { get; set; }
        public string type { get; set; }
    }
}
