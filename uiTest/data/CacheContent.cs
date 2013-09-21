using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SQLite;

namespace uiTest.data
{
    public class CacheContent
    {
        const string TableName = "cacheTags";
        public static void CheckTable()
        {
            if (dataconf.CheckTableExists(TableName))
                return;

            string sql = string.Format(@"CREATE TABLE {0} 
(url varchar(512), Etag varchar(96), lastrequest datetime)",
            TableName);//ID integer primary key autoincrement, 
            dataconf.ExecuteNonQuery(sql);
        }

        public static string ByUrl(string url)
        {
            CheckTable();

            string query = string.Format("select * from {0} where url = @url", TableName);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("url", url);
            Dictionary<string, object> fetched = dataconf.Query(query, parameters).FirstOrDefault();
            if (fetched == null)
                return null;
            return fetched["Etag"].ToString();
        }

        public static void AddModifyRecord(string url, string Etag)
        {
            CheckTable();

            string etag = ByUrl(url);
            if (etag == null)
            {
                // insert
                SQLiteConnection cnn;
                using (cnn = new SQLiteConnection())
                {
                    cnn.ConnectionString = dataconf.connectionstr;
                    cnn.Open();
                    object jmpersistr = dataconf.ExecuteScalar("PRAGMA journal_mode = PERSIST", cnn);

                    //SQLiteTransaction tr = cnn.BeginTransaction();
                    using (SQLiteCommand cmd = cnn.CreateCommand())
                    {
                        //cmd.Transaction = tr;
                        cmd.CommandText = string.Format("INSERT INTO {0} (url, Etag, lastrequest) VALUES(?,?,?)", TableName);

                        dataconf.AddParameter(cmd, url);
                        dataconf.AddParameter(cmd, Etag);
                        dataconf.AddParameter(cmd, DateTime.Now);

                        cmd.ExecuteNonQuery();

                    }
                    //tr.Commit();
                }
            }
            else
            {
                // update
                SQLiteConnection cnn;
                using (cnn = new SQLiteConnection())
                {
                    cnn.ConnectionString = dataconf.connectionstr;
                    cnn.Open();
                    object jmpersistr = dataconf.ExecuteScalar("PRAGMA journal_mode = PERSIST", cnn);

                    //SQLiteTransaction tr = cnn.BeginTransaction();
                    using (SQLiteCommand cmd = cnn.CreateCommand())
                    {
                        //cmd.Transaction = tr;
                        cmd.CommandText = string.Format("UPDATE {0} SET Etag = ?, lastrequest = ? where url = ?", TableName);

                        dataconf.AddParameter(cmd, Etag);
                        dataconf.AddParameter(cmd, DateTime.Now);
                        dataconf.AddParameter(cmd, url);

                        cmd.ExecuteNonQuery();

                    }
                    //tr.Commit();
                }
            }
        }
    }
}
