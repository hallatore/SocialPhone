using System;

namespace SocialPhone
{
    public static class Extensions
    {
        public static string CutEnd(this string input, int limit)
        {
            if (input.Length > limit)
                input = input.Substring(0, limit) + " ...";

            return input;
        }



        public static string ToRelativeDate(this DateTime date)
        {
            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int month = 30 * day;

            var ts = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
            var delta = Math.Abs(ts.TotalSeconds);

            if (delta < 0)
                return "nå";

            if (delta < 1 * minute)
                return ts.Seconds == 1 ? "1 sekund siden" : ts.Seconds + " sekunder siden";

            if (delta < 2 * minute)
                return "1 minutt siden";

            if (delta < 45 * minute)
                return ts.Minutes + " minutter siden";

            if (delta < 90 * minute)
                return "1 time siden";

            if (delta < 24 * hour)
                return ts.Hours + " timer siden";

            if (delta < 48 * hour)
                return "i går";

            if (delta < 30 * day)
                return ts.Days + " dager siden";

            if (delta < 12 * month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "1 måned siden" : months + " måneder siden";
            }

            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "1 år siden" : years + " år siden";
        }
    }
}
