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

        public static byte[] PathToByteArray(string rutaImagen)
        {
            return File.ReadAllBytes(rutaImagen);
        }
        public static int NumberFromFile(string fileName)
        {
            var fileSplit = fileName.Split('\\');
            var lastPart = fileSplit[fileSplit.Length - 1];
            var intStr = "";
            foreach (char c in lastPart)
            {
                if (c == '.') break;
                intStr += c;
            }
            return int.Parse(intStr);
        }

        public static long GetTweetID(string url)
        {
            var tweetID = Regex.Replace(url, "https://twitter.com/.*/status/", "");
            tweetID = Regex.Replace(tweetID, "/.*", "");
            return long.Parse(tweetID);
        }
    }
}
