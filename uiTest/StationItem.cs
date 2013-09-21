using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace uiTest
{
    public class StationItem
    {
        public string Title { get; set; }
        public string City { get; set; }
        public string Direction { get; set; }

        public bool IsUsed { get; set; }
        public string ESR { get; set; }

        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class HistoryItem
    {
        public int CityId;
        public StationItem Start { get; set; }
        public StationItem End { get; set; }
        public DateTime LastRequest { get; set; }

        public string Direction { get { return Start.Direction; } }

        DateTime lastrequest = DateTime.MinValue;
        DateTime departure = DateTime.MinValue;
        string fzzy;
        public TripItem tip = null;

        public string FuzzyTime
        {
            get
            {
                DateTime nowdate = DateTime.Now;
                if ((nowdate - lastrequest).TotalSeconds > 4)
                {
                    lastrequest = nowdate;
                    if (departure == DateTime.MinValue || departure < nowdate)
                    {
                        TimeSpan ts = GetTS(ref nowdate);
                        departure = nowdate + ts;
                    }
                    fzzy = uiTest.FuzzyTime.Compute(departure - nowdate);
                }
                return fzzy;
            }
        }

        private TimeSpan GetTS(ref DateTime nowdate)
        {

            long sec = data.Trips.FindOneClosestSeconds(CityId, Start.ESR, End.ESR, nowdate, out tip);
            TimeSpan ts = TimeSpan.MinValue;
            if (sec < 0)
            {
                ts = nowdate.Date.AddDays(1) - nowdate + (nowdate.AddSeconds(sec) - nowdate.Date);
            }
            else
            {
                ts = TimeSpan.FromSeconds(sec);
            }
            return ts;
        }
    }
    public class TripItem
    {
        public string Title { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public string Duration { get; set; }

        public string Days { get; set; }
        public string Stops { get; set; }

        public string ArrivalPlatform { get; set; }
        public string DeparturePlatform { get; set; }

        public string Currency { get; set; }
        public string Tariff { get; set; }

        public string TrainType { get; set; }
        public string Number { get; set; }

        public int City { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string TrainUID { get; set; }

        public string ClarifiedTariff
        {
            get
            {
                if (!string.IsNullOrEmpty(Tariff))
                    return Tariff;
                return data.Trips.ClarifyTariff(City, from, to, TrainType);
            }
        }

        public bool Express
        {
            get
            {
                return TrainType == "express" || TrainType == "aeroexpress";
            }
        }

        public string TrainTypeRu
        {
            get
            {
                if (TrainType == "express")
                    return "Экпресс";
                if (TrainType == "aeroexpress")
                    return "АэроЭкпресс";

                return "Пригородный";
            }
        }
    }

    public class CityItem
    {
        public string Title { get; set; }
        public int ID { get; set; }
        public string Country { get; set; }
    }
}
