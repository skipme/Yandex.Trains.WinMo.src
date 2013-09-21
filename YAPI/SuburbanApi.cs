using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YAPI
{
    public class SuburbanApi
    {
        public static string UriTrips(string esrfrom, string esrto, string uuid)
        {
            return string.Format("http://mobile.rasp.yandex.net/export/suburban/trip/{0}/{1}/?date={2}&tomorrow_upto=12&uuid={3}", esrfrom, esrto, DateTime.Now.ToString("yyyy-MM-dd"), uuid);
        }

        //public static string UriTrips(string esrfrom, string esrto, string uuid)
        //{
        //    return string.Format("http://mobile.rasp.yandex.net/export/suburban/trip/{0}/{1}/?uuid={2}", esrfrom, esrto, uuid);
        //}

        public static string UriStations(int cityid, string uuid)
        {
            return string.Format("http://mobile.rasp.yandex.net/export/suburban/city/{0}/stations?uuid={1}", cityid, uuid);
        }

        public static string UriUUID()
        {
            return string.Format("http://mobile.rasp.yandex.net/rasp/startup?clid=&app_platform=android&os_version=16&manufacturer=unknown&model=sdk&app_version=200&app_set_region=213&countrycode=310");
        }

        public static string RemoveUidFromUrl(string url)
        {
            int iof = url.IndexOf("uuid") - 1;
            return url.Remove(iof, url.Length - iof);
        }
    }
}
