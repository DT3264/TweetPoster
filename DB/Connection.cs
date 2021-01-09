using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace TweetPoster.DB
{
    public class Connection
    {
        /// <summary>
        /// Obtains the connection url to the DB.
        /// </summary>
        /// <returns>The connection url to the DB</returns>
        public string GetConnectionString()
        {
            
            return Utils.GetEnvVar("MYSQL_URL");
        }

        /// <summary>
        /// Executes an SQL command
        /// </summary>
        /// <param name="sqlCommand">Command to execute</param>
        public void ExecuteSQL(MySqlCommand sqlCommand)
        {
            var connection = new MySqlConnection();
            try
            {
                connection.ConnectionString = GetConnectionString();
                connection.Open();
                sqlCommand.Connection = connection;
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Returns a DataSet.
        /// </summary>
        /// <param name="mysql">The SQL command to execute</param>
        /// <returns>DataSet with the data selected</returns>
        public DataSet GetDataset(string mysql)
        {
            var dataSet = new DataSet();
            var dataAdapter = new MySqlDataAdapter();
            var connection = new MySqlConnection();
            var command = new MySqlCommand();
            try 
            {
                connection.ConnectionString = GetConnectionString();
                connection.Open();
                command.CommandText = mysql;
                dataAdapter.SelectCommand = command;
                dataAdapter.SelectCommand.Connection = connection;
                dataAdapter.Fill(dataSet);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
            return dataSet;
        }
    }
}
