using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TweetPoster
{
    public class Utils
    {
        public static string GetEnvVar(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}
