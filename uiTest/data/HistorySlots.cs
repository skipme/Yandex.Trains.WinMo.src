using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;

namespace uiTest.data
{
    public class HistorySlots
    {
        const int maxslots = 20;

        const string TableName = "hslots";
        public static void CheckTable()
        {
            if (dataconf.CheckTableExists(TableName))
                return;

            string sql = string.Format(@"CREATE TABLE {0} 
(cityId int, esrstart varchar(12), esrend varchar(12), title varchar(255), slotinuse bit, lastrequest datetime)",
            TableName);//ID integer primary key autoincrement, 
            dataconf.ExecuteNonQuery(sql);
        }

        static long RowCount()
        {
            CheckTable();
            long cityisin = (long)dataconf.ExecuteScalar(string.Format(" SELECT COUNT(*) FROM {0}", TableName));

            return cityisin;
        }
        static long RowCountInUsage()
        {
            CheckTable();
            long cityisin = (long)dataconf.ExecuteScalar(string.Format(" SELECT COUNT(*) FROM {0} where slotinuse = 1", TableName));

            return cityisin;
        }
        public static List<HistoryItem> AllSorted()
        {
            CheckTable();

            List<HistoryItem> dsl = new List<HistoryItem>();
            DateTime start = DateTime.Now;
            string sql = string.Format("select * from {0} where slotinuse = 1 order by lastrequest desc", TableName);

            foreach (Dictionary<string, object> dval in uiTest.data.dataconf.Query(sql, null))
            {
                HistoryItem hi = new HistoryItem()
                {
                    Start = Stations.FetchOne(dval["esrstart"].ToString()),
                    End = Stations.FetchOne(dval["esrend"].ToString()),
                    LastRequest = DateTime.Now,
                    CityId = (int)dval["cityId"]
                };
                //hi.FuzzyTime
                dsl.Add(hi);
                //Trips.AllInCityDirection
            }
            return dsl;
        }
        public static void Clear()
        {
            CheckTable();
            string sql = "delete from hslots";
            dataconf.ExecuteNonQuery(sql);
        }
        public static void Remove(HistoryItem hi)
        {
            CheckTable();
            string sql = "delete from hslots where cityId = " + hi.CityId +
                " AND esrstart = '" + hi.Start.ESR + "' AND esrend = '" + hi.End.ESR + "'";
            dataconf.ExecuteNonQuery(sql);
        }
        public static void Push(HistoryItem hi, int city)
        {
            CheckTable();

            string sqlesrs = "select count(*) from {0} where esrstart = '{1}' and esrend = '{2}'";
            sqlesrs = string.Format(sqlesrs, TableName, hi.Start.ESR, hi.End.ESR);
            if ((long)dataconf.ExecuteScalar(sqlesrs) > 0)
            {
                string updtdec = string.Format("update {0} SET lastrequest = '{1}' where esrstart = '{2}' and esrend = '{3}'", TableName, hi.LastRequest.ToString("yyyy-MM-dd HH:mm:ss"), hi.Start.ESR, hi.End.ESR);
                dataconf.ExecuteNonQuery(updtdec);
                return;
            }


            long incindex = RowCountInUsage();
            if (incindex >= maxslots)
            {
                // delete min index
                string delmin = string.Format("delete from {0} where lastrequest in (select min(lastrequest) from {0})", TableName);
                dataconf.ExecuteNonQuery(delmin);

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
                    cmd.CommandText = "INSERT INTO hslots (cityId, esrstart, esrend, title, slotinuse, lastrequest) VALUES(?,?,?,?,?,?)";
                    DbParameter pcityid = cmd.CreateParameter(); cmd.Parameters.Add(pcityid);
                    DbParameter pesrstart = cmd.CreateParameter(); cmd.Parameters.Add(pesrstart);
                    DbParameter pesrend = cmd.CreateParameter(); cmd.Parameters.Add(pesrend);
                    DbParameter ptitle = cmd.CreateParameter(); cmd.Parameters.Add(ptitle);
                    DbParameter pslotinuse = cmd.CreateParameter(); cmd.Parameters.Add(pslotinuse);
                    DbParameter plastrequest = cmd.CreateParameter(); cmd.Parameters.Add(plastrequest);


                    pcityid.Value = city;
                    pesrstart.Value = hi.Start.ESR;
                    pesrend.Value = hi.End.ESR;
                    ptitle.Value = string.Format("{0} - {1}", hi.Start.Title, hi.End.Title);
                    pslotinuse.Value = 1;
                    plastrequest.Value = hi.LastRequest;

                    cmd.ExecuteNonQuery();

                }
                tr.Commit();
            }
        }
    }
}
