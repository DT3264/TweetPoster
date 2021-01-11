using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TweetPoster.DB
{
    public class DAOTweets
    {
         /// <summary>
        /// Inserts a tweet on the DB
        /// </summary>
        /// <param name="tweet"></param>
        public void Insert(Tweet tweet)
        {
            var connection = new Connection();
            
            var SQL = "insert into tweetsscheduled values (null, @captions, @type, @media)";
            var sqlCommand = new MySqlCommand(SQL);
            sqlCommand.Parameters.AddWithValue("@captions", tweet.captions);
            sqlCommand.Parameters.AddWithValue("@type", tweet.type);
            sqlCommand.Parameters.AddWithValue("@media", tweet.media);
            try
            {
                connection.ExecuteSQL(sqlCommand);
            }
            catch(Exception e)
            {
                throw e;
            }

        }
        /// <summary>
        /// Deletes tweets from the database
        /// </summary>
        /// <param name="tweet"></param>
        public void Delete(Tweet tweet)
        {
            var connection = new Connection();
            var SQL = "delete from tweetsscheduled where id=@id";
            var sqlCommand = new MySqlCommand(SQL);
            sqlCommand.Parameters.AddWithValue("@id", tweet.id);
            try
            {
                connection.ExecuteSQL(sqlCommand);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateStatus(Tweet tweet, bool posted)
        {
            var connection = new Connection();
            var SQL = string.Format("update tweetsposted set status='{0}' where id=@id", posted.ToString()[0]);
            var sqlCommand = new MySqlCommand(SQL);
            sqlCommand.Parameters.AddWithValue("@id", tweet.id);
            try
            {
                connection.ExecuteSQL(sqlCommand);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets the topmost tweet scheduled from the DB
        /// </summary>
        /// <returns>The topmost tweet or null if there's no more tweets</returns>
        public Tweet GetNextTweet()
        {
            var connection = new Connection();

            DataSet data;
            try
            {
                data = connection.GetDataset("select * from tweetsscheduled limit 1;");
            }
            catch (Exception e)
            {
                throw e;
            }
            var dataTable = data.Tables[0];

            if (dataTable.Rows.Count == 0) return null;

            var row = dataTable.Rows[0];
            var tweet = new Tweet()
            {
                id = (int)row.ItemArray[0],
                captions = (string)row.ItemArray[1],
                type = (string)row.ItemArray[2],
                media = row.ItemArray[3] is byte[] ?  (byte[])row.ItemArray[3] : null
            };
            return tweet;
        }
    }
}
