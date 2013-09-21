using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;

namespace uiTest.data
{
    public class Cities
    {
        const string TableName = "cities";
        public static void CheckTable()
        {
            if (dataconf.CheckTableExists(TableName))
                return;

            string sql = string.Format(@"CREATE TABLE {0} 
(cityId int, title varchar(255), country varchar(255));
create index City_Title_Index
  on {0} (title collate nocase);
",
            TableName);//ID integer primary key autoincrement, 
            dataconf.ExecuteNonQuery(sql);
        }
        public static List<CityItem> FindByKeyword(string expression)
        {
            CheckTable();
            if (!string.IsNullOrEmpty(expression))
                expression = char.ToUpper(expression[0]) + expression.Remove(0, 1);
            List<CityItem> dsl = new List<CityItem>();

            string sql = "select * from cities where title like @expr order by title";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@expr", "%" + expression + "%");
            foreach (Dictionary<string, object> dval in uiTest.data.dataconf.Query(sql, param))
            {
                dsl.Add(new CityItem()
                {
                    Title = dval["title"].ToString(),
                    ID = (int)dval["cityId"],
                    Country = dval["country"].ToString()
                });
            }

            return dsl;
        }
        public static CityItem FindById(int Id)
        {
            CheckTable();

            List<CityItem> dsl = new List<CityItem>();

            string sql = "select * from cities where cityId = @cid order by title";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@cid", Id);
            foreach (Dictionary<string, object> dval in uiTest.data.dataconf.Query(sql, param))
            {
                dsl.Add(new CityItem()
                {
                    Title = dval["title"].ToString(),
                    ID = (int)dval["cityId"],
                    Country = dval["country"].ToString()
                });
            }

            return dsl.FirstOrDefault();
        }
        public static long RowCount()
        {
            CheckTable();
            long rows = (long)dataconf.ExecuteScalar(string.Format(" SELECT COUNT(*) FROM {0}", TableName));

            return rows;
        }
        public static void Import(YAPI.suburban.suburbancities cities)
        {
            if (cities == null)
                return;

            CheckTable();

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
                    cmd.CommandText = "INSERT INTO cities (cityId, title, country) VALUES(?,?,?)";
                    DbParameter pcityid = cmd.CreateParameter(); cmd.Parameters.Add(pcityid);
                    DbParameter ptitle = cmd.CreateParameter(); cmd.Parameters.Add(ptitle);
                    DbParameter pcountry = cmd.CreateParameter(); cmd.Parameters.Add(pcountry);

                    foreach (YAPI.suburban.suburbancitiesCity c in cities.Items)
                    {
                        pcityid.Value = c.id;
                        ptitle.Value = c.title;
                        pcountry.Value = c.country;
                        cmd.ExecuteNonQuery();
                    }

                }
                tr.Commit();
            }
        }
    }
}
