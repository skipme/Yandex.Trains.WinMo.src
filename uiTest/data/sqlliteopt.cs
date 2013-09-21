using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SQLite;

namespace uiTest.data
{
    public class dataconf
    {
        static string csx = "";
        public static string connectionstr
        {
            get
            {
                lock (csx)
                {
                    if (csx != "")
                        return csx;
                    string path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;

                    csx = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "data.db3");
                    csx = string.Format("Data Source={0}", csx);//;Password=yV8L87emexPX
                    return csx;
                }
            }
        }

        public static DbParameter AddParameter(SQLiteCommand command)
        {
            SQLiteParameter param = command.CreateParameter();
            command.Parameters.Add(param);
            return param;
        }

        public static DbParameter AddParameter(SQLiteCommand command, object value)
        {
            SQLiteParameter param = command.CreateParameter();
            param.Value = value;
            command.Parameters.Add(param);
            return param;
        }

        public static DbParameter AddParameter(string name, SQLiteCommand command)
        {
            SQLiteParameter param = command.CreateParameter();
            param.ParameterName = name;
            command.Parameters.Add(param);
            return param;
        }

        public static bool CheckTableExists(string TableName)
        {
            DbConnection cnn;
            using (cnn = new SQLiteConnection())
            {
                cnn.ConnectionString = dataconf.connectionstr;
                cnn.Open();
                using (DbCommand cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT count(*) FROM sqlite_master WHERE type='table' AND name='{0}'", TableName);
                    long c = (long)cmd.ExecuteScalar();

                    return c != 0;
                }
            }
        }
        public static object ExecuteScalar(string Query)
        {
            DbConnection cnn;
            using (cnn = new SQLiteConnection())
            {
                cnn.ConnectionString = dataconf.connectionstr;
                cnn.Open();
                using (DbCommand cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = Query;
                    object o = cmd.ExecuteScalar();
                    if (o == DBNull.Value)
                        return null;
                    return o;
                }
            }
        }
        public static object ExecuteScalar(string Query, SQLiteConnection cnn)
        {
            using (DbCommand cmd = cnn.CreateCommand())
            {
                cmd.CommandText = Query;
                object o = cmd.ExecuteScalar();
                if (o == DBNull.Value)
                    return null;
                return o;
            }
        }
        public static void ClearTable(string Table)
        {
            ExecuteNonQuery(string.Format("delete from {0}", Table));
        }
        public static void ExecuteNonQuery(string Query)
        {
            DbConnection cnn;
            using (cnn = new SQLiteConnection())
            {
                cnn.ConnectionString = dataconf.connectionstr;
                cnn.Open();
                using (DbCommand cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = Query;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static IEnumerable<Dictionary<string, object>> Query(string sql, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            using (DbConnection connection = new SQLiteConnection())
            {
                connection.ConnectionString = dataconf.connectionstr;

                DbDataReader reader = null;

                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = sql;
                if (parameters != null)
                    foreach (KeyValuePair<string, object> kv in parameters)
                    {
                        DbParameter p = command.CreateParameter();
                        p.Value = kv.Value;
                        p.ParameterName = kv.Key;
                        command.Parameters.Add(p);
                    }
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> data = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            data.Add(reader.GetName(i), reader[i]);
                        }
                        yield return data;
                    }
                    reader.Close();
                }
                else yield break;
            }
        }
    }
}
