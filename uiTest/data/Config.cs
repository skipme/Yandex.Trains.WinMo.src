using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;

namespace uiTest.data
{
    public class Config
    {
        public int City { get; set; }

        const string TableName = "configuration";
        public static void CheckTable()
        {
            if (dataconf.CheckTableExists(TableName))
                return;

            string sql = string.Format(@"CREATE TABLE {0} 
(cityId int)",
            TableName);//ID integer primary key autoincrement, 
            dataconf.ExecuteNonQuery(sql);
        }
        public static Config Fetch()
        {
            CheckTable();

            string sql = "select * from configuration";
            Dictionary<string, object> fetched = uiTest.data.dataconf.Query(sql, null).FirstOrDefault();
            if (fetched == null)
                return null;
            Config inst = new Config()
            {
                City = (int)fetched["cityId"]
            };
            return inst;
        }

        public void Save()
        {
            CheckTable();
            Config inst = Fetch();
            if (inst != null)
            {
                data.dataconf.ExecuteNonQuery("delete from configuration");
            }
            SQLiteConnection cnn;
            using (cnn = new SQLiteConnection())
            {
                cnn.ConnectionString = dataconf.connectionstr;
                cnn.Open();
                object jmpersistr = dataconf.ExecuteScalar("PRAGMA journal_mode = PERSIST", cnn);

                DbTransaction tr = cnn.BeginTransaction();
                using (DbCommand cmd = cnn.CreateCommand())
                {
                    cmd.Transaction = tr;
                    cmd.CommandText = "INSERT INTO configuration (cityId) VALUES(?)";
                    DbParameter pcityid = cmd.CreateParameter(); cmd.Parameters.Add(pcityid);

                    pcityid.Value = this.City;

                    cmd.ExecuteNonQuery();

                }
                tr.Commit();
            }
        }
    }
}
