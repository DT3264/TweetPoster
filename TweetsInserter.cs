using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TweetPoster.DB;

namespace TweetPoster
{
    class TweetsInserter
    {
        public static void InsertTweets(string filesPath, string urlsPath)
        {
            var dao = new DAOTweets();
            var files = Directory.GetFiles(filesPath, "*.*");
            var urls = File.ReadAllLines(urlsPath);
            Array.Sort(files, (x,y)=> {
                int vX = Utils.NumberFromFile(x);
                int vY = Utils.NumberFromFile(y);
                return vX < vY ? -1 : 1;
            });

            for(int i=0; i<files.Length; i++)
            {
                string file = files[i];
                string url = urls[i];
                Tweet tweet;

                if (url.Contains("twitter"))
                {
                    var tweetID = Utils.GetTweetID(url);
                    tweet = new Tweet()
                    {
                        id = -1,
                        captions = tweetID.ToString(),
                        media = null,
                        type = "retweet"
                    };
                }
                else
                {
                    var image = Utils.PathToByteArray(file);
                    tweet = new Tweet()
                    {
                        id = -1,
                        captions = url,
                        media = image,
                        type = "post"
                    };
                }
                dao.Insert(tweet);

                Console.WriteLine(string.Format("Insertado {0} de {1}", i+1, files.Length));
            }
        }
    }
}
