using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;

namespace uiTest.data
{
    public class Trips
    {
        const string TableName = "trips";
        public static void CheckTable()
        {
            if (dataconf.CheckTableExists(TableName))
                return;

            string sql = string.Format(@"CREATE TABLE {0} 
(ID integer primary key autoincrement, cityId int, esrfrom varchar(12), esrto varchar(12), duration int, stops varchar(1024), title varchar(512), arrival int, arrival_platform varchar(255), days varchar(255), departure int, departure_platform varchar(255), currency varchar(8), tnum varchar(16), tariff varchar(32), ttype varchar(32), tuid varchar(32));
create index FindOneClosestSeconds
  on {0} (cityId collate nocase, esrfrom, esrto);
",
            TableName);
            dataconf.ExecuteNonQuery(sql);
        }

        public static List<TripItem> AllInCityDirection(int cityId, string esrfrom, string esrto)
        {
            CheckTable();

            List<TripItem> result = new List<TripItem>();
            foreach (Dictionary<string, object> segment in dataconf.Query(string.Format(" SELECT * FROM {0} WHERE cityId = {1} AND esrfrom = '{2}' AND esrto = '{3}'", TableName, cityId, esrfrom, esrto), null))
            {
                int arrivalmin = int.Parse(segment["arrival"].ToString());
                int depmin = int.Parse(segment["departure"].ToString());
                DateTime nowdate = DateTime.Now.Date;
                result.Add(new TripItem()
                {
                    Arrival = nowdate.AddMinutes(arrivalmin),
                    Duration = segment["duration"].ToString(),
                    Departure = nowdate.AddMinutes(depmin),
                    Title = segment["title"].ToString(),
                    Days = segment["days"].ToString(),
                    Stops = segment["stops"].ToString(),
                    ArrivalPlatform = segment["arrival_platform"].ToString(),
                    DeparturePlatform = segment["departure_platform"].ToString(),
                    Currency = segment["currency"].ToString(),
                    Number = segment["tnum"].ToString(),
                    Tariff = segment["tariff"].ToString(),
                    City = cityId,
                    from = esrfrom,
                    to = esrto,
                    TrainType = segment["ttype"].ToString(),
                    TrainUID = segment["tuid"].ToString()
                });
            }
            return result;
        }

        public static TripItem ById(int id)
        {
            CheckTable();

            Dictionary<string, object> segment = dataconf.Query(string.Format(" SELECT * FROM {0} WHERE ID = {1} ", TableName, id), null).FirstOrDefault();

            int arrivalmin = int.Parse(segment["arrival"].ToString());
            int depmin = int.Parse(segment["departure"].ToString());
            DateTime nowdate = DateTime.Now.Date;
            return new TripItem()
            {
                Arrival = nowdate.AddMinutes(arrivalmin),
                Duration = segment["duration"].ToString(),
                Departure = nowdate.AddMinutes(depmin),
                Title = segment["title"].ToString(),
                Days = segment["days"].ToString(),
                Stops = segment["stops"].ToString(),
                ArrivalPlatform = segment["arrival_platform"].ToString(),
                DeparturePlatform = segment["departure_platform"].ToString(),
                Currency = segment["currency"].ToString(),
                Number = segment["tnum"].ToString(),
                Tariff = segment["tariff"].ToString(),
                City = (int)segment["cityId"],
                from = segment["esrfrom"].ToString(),
                to = segment["esrto"].ToString(),
                TrainType = segment["ttype"].ToString(),
                TrainUID = segment["tuid"].ToString()
            };
        }

        public static long FindOneClosestSeconds(int cityId, string esrfrom, string esrto, DateTime point, out TripItem tic)
        {
            CheckTable();

            tic = null;

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@datenow", point.Date);
            parameters.Add("@datetimenow", point);
            parameters.Add("@city", cityId);
            parameters.Add("@esrstart", esrfrom);
            parameters.Add("@esrend", esrto);
            const string sqll = @"select ID, {0}(delta) seconds from
(
select ID,
 datetime(date('now'),'+'||arrival||' minute') arrivaltime,
 strftime('%s', datetime(@datenow,'+'||departure||' minute')) - strftime('%s',@datetimenow) delta
 from trips
where cityid = @city
and esrfrom = @esrstart 
and esrto = @esrend
) tripsin
";
            string deltam = string.Format(sqll, "MIN") + " where tripsin.delta > 0 ";

            Dictionary<string, object> result = dataconf.Query(deltam, parameters).FirstOrDefault();
            if (result == null || result.Count == 0)
                return 0;
            else
            {
                object seco = result["seconds"];
                int id = -1;
                if (seco == DBNull.Value)
                {
                    deltam = string.Format(sqll, "MIN") + " where tripsin.delta < 0 ";
                    result = dataconf.Query(deltam, parameters).FirstOrDefault();
                    if (result == null || result.Count == 0)
                        return 0;
                    seco = result["seconds"];
                    
                    if (seco == DBNull.Value)
                        return 0;
                    else id = (int)(long)result["ID"];
                }
                else id = (int)(long)result["ID"];
                tic = ById(id);
                return (long)result["seconds"];
            }
        }

        public static string ClarifyTariff(int cityId, string esrfrom, string esrto, string ttype)
        {
            CheckTable();

            string request = @"select tariff, ttype, tuid from trips where cityId = @c and esrfrom = @ef and esrto = @et group by tariff, ttype";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@c", cityId);
            param.Add("@ef", esrfrom);
            param.Add("@et", esrto);

            string mny = "";
            int mnycnt = 0;
            foreach (Dictionary<string, object> result in data.dataconf.Query(request, param))
            {
                if (result["ttype"].ToString() == ttype)
                {
                    if (mnycnt >= 1)
                        mny += ", ";
                    string tmny = result["tariff"].ToString();
                    string tuid = result["tuid"].ToString();
                    if (!string.IsNullOrEmpty(tuid))
                        mny += tuid;
                    if (!string.IsNullOrEmpty(tmny))
                    {
                        mny += tmny;
                        mnycnt++;
                    }
                }
            }

            return mny;
        }

        public static List<StationItem> SearchInStations(int cityId, string word)
        {
            return null;
        }

        public static void Import(YAPI.suburban.trip stat, int cityId, string esrfrom, string esrto)
        {
            CheckTable();

            long cityisin = (long)dataconf.ExecuteScalar(string.Format(" SELECT COUNT(*) FROM {0} WHERE cityId = {1} AND esrfrom = '{2}' AND esrto = '{3}'", TableName, cityId, esrfrom, esrto));
            if (stat.segment == null || stat.segment.Length == 0)
            {
                return;
            }
            if (cityisin > 0)
            {
                string sql_removeunusable = @"delete from {0} WHERE cityId = {1} AND esrfrom = '{2}' AND esrto = '{3}' ";
                sql_removeunusable = string.Format(sql_removeunusable, TableName, cityId, esrfrom, esrto);
                dataconf.ExecuteNonQuery(sql_removeunusable);
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
                    cmd.CommandText = "INSERT INTO trips (cityId, esrfrom, esrto, duration, stops, title, arrival, arrival_platform, days, departure, departure_platform, currency, tnum, tariff, ttype, tuid) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                    DbParameter pcityid = cmd.CreateParameter(); cmd.Parameters.Add(pcityid);
                    DbParameter pesrfrom = cmd.CreateParameter(); cmd.Parameters.Add(pesrfrom);
                    DbParameter pesrto = cmd.CreateParameter(); cmd.Parameters.Add(pesrto);
                    DbParameter pduration = cmd.CreateParameter(); cmd.Parameters.Add(pduration);
                    DbParameter pstops = cmd.CreateParameter(); cmd.Parameters.Add(pstops);
                    DbParameter ptitle = cmd.CreateParameter(); cmd.Parameters.Add(ptitle);
                    DbParameter parrival = cmd.CreateParameter(); cmd.Parameters.Add(parrival);
                    DbParameter parrival_platf = cmd.CreateParameter(); cmd.Parameters.Add(parrival_platf);
                    DbParameter pdays = cmd.CreateParameter(); cmd.Parameters.Add(pdays);
                    DbParameter pdeparture = cmd.CreateParameter(); cmd.Parameters.Add(pdeparture);
                    DbParameter pdeparture_platf = cmd.CreateParameter(); cmd.Parameters.Add(pdeparture_platf);
                    DbParameter pcurrency = cmd.CreateParameter(); cmd.Parameters.Add(pcurrency);
                    DbParameter ptnum = cmd.CreateParameter(); cmd.Parameters.Add(ptnum);
                    DbParameter ptariff = cmd.CreateParameter(); cmd.Parameters.Add(ptariff);
                    DbParameter pttype = cmd.CreateParameter(); cmd.Parameters.Add(pttype);
                    DbParameter tuid = cmd.CreateParameter(); cmd.Parameters.Add(tuid);

                    foreach (YAPI.suburban.tripSegment s in stat.segment)
                    {
                        pesrfrom.Value = stat.from;
                        pesrto.Value = stat.to;
                        pduration.Value = s.duration;
                        ptitle.Value = s.title;
                        pstops.Value = s.stops;

                        parrival.Value = TimeToMin(s.arrival);
                        parrival_platf.Value = s.arrival_platform;
                        pdays.Value = s.days;
                        pdeparture.Value = TimeToMin(s.departure);
                        pdeparture_platf.Value = s.departure_platform;

                        pcurrency.Value = s.currency;
                        ptnum.Value = s.number;
                        ptariff.Value = s.tariff;
                        pttype.Value = s.type;
                        pcityid.Value = cityId;
                        tuid.Value = s.uid;

                        cmd.ExecuteNonQuery();
                    }
                }
                tr.Commit();
            }
        }

        static int TimeToMin(string t)
        {
            int spacei = 0;
            if ((spacei = t.IndexOf(' ')) > 0) { t = t.Substring(spacei, t.Length - spacei); }
            string[] hhmm = t.Split(':');
            int hh = int.Parse(hhmm[0]);
            int mm = int.Parse(hhmm[1]);
            return hh * 60 + mm;
        }
    }
}
