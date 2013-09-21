using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;

namespace uiTest.data
{
    public class Stations
    {
        const string TableName = "stations";
        public static void CheckTable()
        {
            if (dataconf.CheckTableExists(TableName))
                return;

            string sql = string.Format(@"CREATE TABLE {0} 
(ID integer primary key autoincrement, direction varchar(255), lat numeric(2,6), lon numeric(2,6), title varchar(255), city varchar(255), esr varchar(12), cityid int);
create index Station_Title_Index
  on {0} (title collate nocase);",
            TableName);
            dataconf.ExecuteNonQuery(sql);
        }

        public static List<StationItem> AllInCityDirection(int cityId, string direction)
        {
            List<StationItem> dsl = new List<StationItem>();
            DateTime start = DateTime.Now;
            string sql = "select * from stations where cityid = @city and direction = @dir order by title";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@dir", direction);
            param.Add("@city", cityId);
            foreach (Dictionary<string, object> dval in uiTest.data.dataconf.Query(sql, param))
            {
                dsl.Add(new StationItem()
                {
                    Title = dval["title"].ToString(),
                    City = dval["city"].ToString() + " " + dval["direction"].ToString(),
                    ESR = dval["esr"].ToString(),
                    lat = (double)(decimal)dval["lat"],
                    lon = (double)(decimal)dval["lon"]
                });
            }
            System.Diagnostics.Debug.WriteLine("fetch stations in direction: " + (DateTime.Now - start).TotalSeconds);
            return dsl;
        }

        public static StationItem FetchOne(string esr)
        {
            List<StationItem> dsl = new List<StationItem>();
            DateTime start = DateTime.Now;
            string sql = "select * from stations where esr = @esr ";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@esr", esr);

            foreach (Dictionary<string, object> dval in uiTest.data.dataconf.Query(sql, param))
            {
                dsl.Add(new StationItem()
                {
                    Title = dval["title"].ToString(),
                    City = dval["city"].ToString() + " " + dval["direction"].ToString(),
                    ESR = dval["esr"].ToString(),
                    lat = (double)(decimal)dval["lat"],
                    lon = (double)(decimal)dval["lon"]
                });
            }
            System.Diagnostics.Debug.WriteLine("fetch stations in esr: " + (DateTime.Now - start).TotalSeconds);

            return dsl[0];
        }

        public static List<StationItem> SearchInStations(int cityId, string expression)
        {
            List<StationItem> stats = SearchInStationsD(cityId, expression);
            if (!string.IsNullOrEmpty(expression))
                expression = char.ToUpper(expression[0]) + expression.Remove(0, 1);

            foreach (StationItem st in SearchInStationsD(cityId, expression))
            {
                bool ex = false;
                foreach (StationItem x in stats)
                {
                    if (st.ESR == x.ESR)
                    { ex = true; break; }
                }
                if (!ex)
                    stats.Add(st);
            }


            return stats;
        }
        public static List<StationItem> SearchInStationsD(int cityId, string expression)
        {
            List<StationItem> dsl = new List<StationItem>();
            DateTime start = DateTime.Now;
            string sql = "select * from stations where cityid = @city and title like @expr order by title";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@expr", "%" + expression + "%");
            param.Add("@city", cityId);
            foreach (Dictionary<string, object> dval in uiTest.data.dataconf.Query(sql, param))
            {
                dsl.Add(new StationItem()
                {
                    Title = dval["title"].ToString(),
                    City = dval["city"].ToString() + " " + dval["direction"].ToString(),
                    ESR = dval["esr"].ToString(),
                    lat = (double)(decimal)dval["lat"],
                    lon = (double)(decimal)dval["lon"]
                });
            }
            return dsl;
        }
        public static List<string> DirectionsInCity(int cityId)
        {
            if (RowCount(cityId) == 0)
                return null;

            List<string> dirs = new List<string>();

            string sql = "select direction from {0} where cityId = {1} group by direction order by direction";
            sql = string.Format(sql, TableName, cityId);
            foreach (Dictionary<string, object> d in dataconf.Query(sql, null))
            {
                dirs.Add(d["direction"] as string);
            }
            return dirs;
        }
        static long RowCount(int cityId)
        {
            CheckTable();
            long cityisin = (long)dataconf.ExecuteScalar(string.Format(" SELECT COUNT(*) FROM {0} WHERE cityId = {1}", TableName, cityId));

            return cityisin;
        }
        public static void Import(YAPI.suburban.citystations stat, int cityId)
        {
            long cityisin = RowCount(cityId);
            if (cityisin > 0 || stat.Items.Length == 0)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("] ignore import for city: {0}, counts:{1}, to import:{2}", cityId, cityisin, stat.Items.Length));
                return;
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
                    cmd.CommandText = "INSERT INTO stations (direction, lat, lon, title, city, esr, cityid) VALUES(?,?,?,?,?,?,?)";
                    DbParameter pd = cmd.CreateParameter(); cmd.Parameters.Add(pd);
                    DbParameter plon = cmd.CreateParameter(); cmd.Parameters.Add(plon);
                    DbParameter plat = cmd.CreateParameter(); cmd.Parameters.Add(plat);
                    DbParameter ptitle = cmd.CreateParameter(); cmd.Parameters.Add(ptitle);
                    DbParameter pcity = cmd.CreateParameter(); cmd.Parameters.Add(pcity);
                    DbParameter pesr = cmd.CreateParameter(); cmd.Parameters.Add(pesr);
                    DbParameter pcityid = cmd.CreateParameter(); cmd.Parameters.Add(pcityid);
                    foreach (YAPI.suburban.citystationsStation s in stat.Items)
                    {
                        pd.Value = s.direction;
                        plon.Value = s.lon;
                        plat.Value = s.lat;
                        ptitle.Value = s.title;
                        pcity.Value = s.city;
                        pesr.Value = s.esr;
                        pcityid.Value = cityId;

                        cmd.ExecuteNonQuery();
                    }
                }
                tr.Commit();
            }
        }


    }
}
