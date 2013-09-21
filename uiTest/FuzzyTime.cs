using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace uiTest
{
    public class FuzzyTime
    {
        static string[] mapmin = new string[] { 
            "минут",
            "минуту",
            "минуты",
            "минуты",
            "минуты",
            "минут",
            "минут",
            "минут",
            "минут",
            "минут",
        };
        static string[] mapsec = new string[] { 
            "секунд",
            "секунду",
            "секунды",
            "секунды",
            "секунды",
            "секунд",
            "секунд",
            "секунд",
            "секунд",
            "секунд",
        };
        static string[] maphour = new string[] { 
            "часов",
            "час",
            "часа",
            "часа",
            "часа",
            "часов",
            "часов",
            "часов",
            "часов",
            "часов",
        };
        public static string Compute(TimeSpan ts)
        {
            int delta = (int)ts.TotalSeconds;
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            string answr;
            const string prepend = "через ";

            if (delta < 0)
            {
                answr = "not yet";
            }
            else
                if (delta < 1 * MINUTE)
                {
                    answr = ts.TotalSeconds <= 15 ? "сейчас" : prepend + ts.Seconds + " " + map(ts.Seconds, mapsec);
                }
                else
                    if (delta < 2 * MINUTE)
                    {
                        answr = prepend + "минуту";
                    }
                    else
                        if (delta < 45 * MINUTE)
                        {
                            answr = prepend + (ts.Minutes + " " + map(ts.Minutes, mapmin));
                        }
                        else
                            if (delta < 90 * MINUTE)
                            {
                                answr = prepend + "час";
                            }
                            else
                                if (delta < 24 * HOUR)
                                {
                                    answr = prepend + (ts.Hours + " " + map(ts.Hours, maphour));
                                }
                                else
                                    if (delta < 48 * HOUR)
                                    {
                                        answr = "завтра";
                                    }
                                    else
                                        if (delta < 30 * DAY)
                                        {
                                            answr = prepend + (ts.Days + " дней");
                                        }
                                        else
                                            if (delta < 12 * MONTH)
                                            {
                                                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                                                answr = prepend + (months <= 1 ? " месяц" : months + " месяцев");
                                            }
                                            else
                                            {
                                                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                                                answr = prepend + (years <= 1 ? " год" : years + " лет");
                                            }

            return answr;
        }

        static string map(int v, string[] xmap)
        {
            string z = v.ToString();
            if (z.Length >= 1)
            {
                int x = int.Parse(z[z.Length - 1].ToString());
                return xmap[x];
            }
            else
            {
                return xmap[0];
            }
        }
    }
}
