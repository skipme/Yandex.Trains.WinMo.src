using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using YAPI.suburban;

namespace uiTest
{
    public class SuburbanContext
    {
        public SuburbanContext()
        {
            CitySelected = 213;
            uuid = null;
        }

        static bool CheckUUID()
        {
            if (instance.uuid == null)
            {
                htmlRetrieval.WebPage page = new htmlRetrieval.WebPage();
                page.Address = YAPI.SuburbanApi.UriUUID();
                YAPI.suburban.startup t = YAPI.suburban.startup.FromXml(page);
                if (t == null)
                {
                    instance.StateErrorNetwork = true;
                    return false;
                }
                instance.uuid = t.uuid;

            }
            return true;
        }

        static SuburbanContext instance = new SuburbanContext();

        public string uuid { get; set; }
        public int CitySelected { get; set; }
        public string DirectionSelected { get; set; }

        public bool StateErrorNetwork { get; set; }
        public string StateErrorNetworkString { get; set; }

        public StationItem StationStart { get; set; }
        public StationItem StationEnd { get; set; }

        public bool WorkOffline { get; set; }
        public bool SearchStationFrom { get; set; }

        public static void SetCity(int City)
        {
            if (instance.CitySelected != City)
            {
                instance.CitySelected = City;
            }            
        }
        public static bool SearchListStationFrom { get { return instance.SearchStationFrom; } }
        public static int GetCity()
        {
            return instance.CitySelected;
        }
        public static void SetDir(string Direction)
        {
            instance.DirectionSelected = Direction;
        }

        public static void SetStart(StationItem From)
        {
            instance.StationStart = From;
            instance.DirectionSelected = From.Direction;
        }

        public static void SetEnd(StationItem To)
        {
            instance.StationEnd = To;
        }

        public static bool WorkOffLine { get { return instance.WorkOffline; } set { instance.WorkOffline = value; } }
        public static bool NetworkNA { get { return instance.StateErrorNetwork; } }

        public static List<TripItem> FindTrips(DateTime origin)
        {
            return FindTrips(origin, false);
        }
        public static List<TripItem> FindTrips(DateTime origin, bool sync)
        {
            instance.StateErrorNetwork = false;
            // check for data contains in tables
            List<TripItem> trips = null;
            if (!sync)
                trips = data.Trips.AllInCityDirection(instance.CitySelected, instance.StationStart.ESR, instance.StationEnd.ESR);
            if (sync || trips.Count == 0)
            {
                if (WorkOffLine)
                    return null;
                else
                {
                    if (!CheckUUID())
                        return null;

                    htmlRetrieval.WebPage page = new htmlRetrieval.WebPage();
                    page.Address = YAPI.SuburbanApi.UriTrips(instance.StationStart.ESR, instance.StationEnd.ESR, instance.uuid);
                    page.RequestEtag = data.CacheContent.ByUrl(YAPI.SuburbanApi.RemoveUidFromUrl(page.Address));
                    string xml = page.Html;

                    if (page.NotModified)
                    {
                        return data.Trips.AllInCityDirection(instance.CitySelected, instance.StationStart.ESR, instance.StationEnd.ESR);
                    }
                    YAPI.suburban.trip t = YAPI.suburban.trip.FromXml(xml);
                    if (t == null)
                    {
                        // network is offline
                        instance.StateErrorNetwork = true;
                    }
                    else
                    {
                        data.CacheContent.AddModifyRecord(YAPI.SuburbanApi.RemoveUidFromUrl(page.Address), page.ResponseEtag);
                        data.Trips.Import(t, instance.CitySelected, instance.StationStart.ESR, instance.StationEnd.ESR);
                        trips = data.Trips.AllInCityDirection(instance.CitySelected, instance.StationStart.ESR, instance.StationEnd.ESR);
                    }
                }
            }

            data.HistorySlots.Push(new HistoryItem()
            {
                Start = instance.StationStart,
                End = instance.StationEnd,
                LastRequest = DateTime.Now
            }, instance.CitySelected);

            return trips;
        }

        public static List<string> availableDirections()
        {
            instance.StateErrorNetwork = false;

            List<string> dirs = data.Stations.DirectionsInCity(instance.CitySelected);
            if (dirs == null)
            {
                htmlRetrieval.WebPage page = new htmlRetrieval.WebPage();
                page.FromFile(string.Format("stations_{0}.xml", instance.CitySelected));

                if (WorkOffLine)
                    return null;
                else
                {
                    if (page.Html == null)
                    {
                        if (!CheckUUID())
                            return null;
                        page.Address = YAPI.SuburbanApi.UriStations(instance.CitySelected, instance.uuid);
                    }
                    YAPI.suburban.citystations cs = YAPI.suburban.citystations.FromXml(page);
                    if (cs == null)
                    {
                        // network is offline
                        instance.StateErrorNetwork = true;
                    }
                    else
                    {
                        data.Stations.Import(cs, instance.CitySelected);
                        dirs = data.Stations.DirectionsInCity(instance.CitySelected);
                    }
                }

            }
            return dirs;
        }

        public static List<StationItem> SearchStation(string expression)
        {
            return data.Stations.SearchInStations(instance.CitySelected, expression);
        }
        public static List<StationItem> SearchStationDirection(string direction)
        {
            return data.Stations.AllInCityDirection(instance.CitySelected, direction);
        }

        public static List<CityItem> SearchCity(string expression)
        {
            long cities = data.Cities.RowCount();
            if (cities == 0)
            {
                htmlRetrieval.WebPage wp = new htmlRetrieval.WebPage();
                wp.FromFile("suburban_cities.xml");
                data.Cities.Import(YAPI.suburban.suburbancities.FromXml(wp));
            }
            return data.Cities.FindByKeyword(expression);
        }
        public static CityItem SearchCity(int Id)
        {
            if (Id == -1)
                Id = instance.CitySelected;
            long cities = data.Cities.RowCount();
            if (cities == 0)
            {
                htmlRetrieval.WebPage wp = new htmlRetrieval.WebPage();
                wp.FromFile("suburban_cities.xml");
                data.Cities.Import(YAPI.suburban.suburbancities.FromXml(wp));
            }
            return data.Cities.FindById(Id);
        }
        public static string CurrentRequest
        {
            get
            {
                return string.Format("{0} {1}", instance.StationStart.Title, instance.StationEnd.Title);
            }
        }

        public static void SearchStation(bool From)
        {
            instance.SearchStationFrom = From;
        }
        public static void SetStationFromSearch(StationItem i)
        {
            if (instance.SearchStationFrom)
                SetStart(i);
            else SetEnd(i);
        }
    }
}
