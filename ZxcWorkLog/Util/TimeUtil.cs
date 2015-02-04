using System;
using System.Text.RegularExpressions;

namespace ZxcWorkLog.Util
{
    static class TimeUtil
    {
        public static string ToReadableTime(long ticks, bool showSeconds = true, bool showDays = true)
        {
            var timeSpan = new TimeSpan(ticks);
            long days = timeSpan.Days;
            long hours = timeSpan.Hours;
            long minutes = timeSpan.Minutes;
            long seconds = timeSpan.Seconds;

            if (!showDays)
            {
                hours += days * 24;
                days = 0;
            }

            string result = "";
            if (days > 0) result += days + "d ";
            if (hours > 0) result += hours + "h ";
            if (minutes > 0) result += minutes + "m ";
            if (seconds > 0 && showSeconds) result += seconds + "s";

            if (result != "") return result.TrimEnd(new []{' '});
            return showSeconds ? "0s" : "0m";
        }

        public static TimeSpan FromReadableTime(string text)
        {
            Console.WriteLine(text);

            Regex r = new Regex(@"(\d+)(\w+)");

            int days = 0, hours = 0, minutes = 0, seconds = 0;

            Match m = r.Match(text);
            while (m.Success)
            {
                var number = m.Groups[1].Captures[0].Value;
                var letter = m.Groups[2].Captures[0].Value;
                
                int num;
                if (Int32.TryParse(number, out num))
                {
                    switch (letter)
                    {
                        case "d":
                            days = num;
                            break;
                        case "h":
                            hours = num;
                            break;
                        case "m":
                            minutes = num;
                            break;
                        case "s":
                            seconds = num;
                            break;
                    }
                }
                m = m.NextMatch();
            }
            return new TimeSpan(days, hours, minutes, seconds);
        }
    }
}
