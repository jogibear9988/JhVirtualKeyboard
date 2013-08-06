// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;


namespace JhLib
{
    /// <summary>
    /// This type denotes a simplistic, course frequency, such as for how often to do something.
    /// Values range from EveryMinute to Hourly.
    /// </summary>
    public enum TimeIntervalRate { Hourly = 0, Every30Minutes, Every15Minutes, Every10Minutes, Every5Minutes, EveryMinute };

    public static class TimeLib
    {
        #region AsTimeInterval
        /// <summary>
        /// Return a string respresenting (in English) the time interval bounded by t1 and t2.
        /// </summary>
        /// <param name="earliestTime">The earlier of the two DateTimes</param>
        /// <param name="latestTime">The latter of the two DateTimes</param>
        public static string AsTimeInterval(this DateTime? earliestTime, DateTime? latestTime)
        {
            string sResult;
            if (earliestTime == null && latestTime == null)
            {
                sResult = "? .. ?";
            }
            else if (earliestTime == null)
            {
                sResult = "? .. " + latestTime.Value.ToStringMinimum();
            }
            else if (latestTime == null)
            {
                sResult = earliestTime.Value.ToStringWithoutYearIfSame() + " .. ?";
            }
            else // neither is null.
            {
                DateTime t1 = earliestTime.Value;
                DateTime t2 = latestTime.Value;
                if (t1.IsSameDayAs(t2))
                {
                    if (t1.Hour == t2.Hour && t1.Minute == t2.Minute)
                    {
                        // Same minute. Are the seconds the same?
                        if (t1.Second == t2.Second)
                        {
                            // They are the same right down to the second, so only show one time.
                            sResult = earliestTime.Value.ToStringMinimum();
                        }
                        else // second is not the same.
                        {
                            // Show the 2nd time without the date portion.
                            // Only the seconds component distinquishes them, so show the seconds.
                            sResult = earliestTime.Value.ToStringMinimum(true) + " .. " + String.Format("{0:h:mm:sstt}", latestTime);
                        }
                    }
                    else // not the same minute.
                    {
                        // Show the 2nd time without the date portion.
                        sResult = earliestTime.Value.ToStringMinimum() + " .. " + String.Format("{0:h:mmtt}", latestTime);
                    }
                }
                else // not the same day.
                {
                    if (t1.Year == t2.Year)
                    {
                        // Show the 2nd time without the year portion.
                        sResult = String.Format(_sDateTimeFormat, earliestTime) + " .. " + String.Format("{0:M-dd h:mmtt}", latestTime);
                    }
                    else
                    {
                        // Show the complete long form.
                        sResult = String.Format(_sDateTimeFormat, earliestTime) + " .. " + String.Format(_sDateTimeFormat, latestTime);
                    }
                }
            }
            return sResult;
        }
        #endregion AsTimeInterval

        #region SecondsToIntervalBoundary
        /// <summary>
        /// Given a TimeIntervalRate, return the number of seconds until the next actual occurance
        /// of that boundary. Ie, if the rate is hourly, and it's presently 3:59pm, return 60.
        /// </summary>
        /// <param name="eEveryNMinutes">an enum that expresses a rate</param>
        /// <returns>The number of seconds before the next interval boundary</returns>
        public static int SecondsToIntervalBoundary(TimeIntervalRate eEveryNMinutes)
        {
            int iEveryNMinutes = 60;
            switch (eEveryNMinutes)
            {
                case TimeIntervalRate.EveryMinute:
                    iEveryNMinutes = 1;
                    break;
                case TimeIntervalRate.Every5Minutes:
                    iEveryNMinutes = 5;
                    break;
                case TimeIntervalRate.Every10Minutes:
                    iEveryNMinutes = 10;
                    break;
                case TimeIntervalRate.Every15Minutes:
                    iEveryNMinutes = 15;
                    break;
                case TimeIntervalRate.Every30Minutes:
                    iEveryNMinutes = 30;
                    break;
                case TimeIntervalRate.Hourly:
                    iEveryNMinutes = 60;
                    break;
            }
            return SecondsToIntervalBoundary(iEveryNMinutes);
        }

        /// <summary>
        /// Given a value representing how many minutes per interval,
        /// return the number of seconds until the next actual occurance
        /// of that interval boundary.
        /// </summary>
        /// <param name="iEveryNMinutes"></param>
        /// <returns></returns>
        public static int SecondsToIntervalBoundary(int iEveryNMinutes)
        {
            int iSecondsToGo = 0;
            DateTime tNow = DateTime.Now;
            int m = tNow.Minute % iEveryNMinutes;
            int iMinutesLater = iEveryNMinutes - m;
            int iSecondsLater = 0;
            if (tNow.Second > 1)
            {
                if (iMinutesLater == 0)
                {
                    iMinutesLater = iEveryNMinutes - 1;
                }
                else
                {
                    iMinutesLater--;
                }
                iSecondsLater = 60 - tNow.Second;
            }
            iSecondsToGo = iMinutesLater * 60 + iSecondsLater;
            return iSecondsToGo;
        }
        #endregion

        #region SecondsForInterval
        /// <summary>
        /// Return the number of seconds that corresponds to the given TimeIntervalRate,
        /// e.g. for EveryMinute return 60.
        /// </summary>
        public static int SecondsForInterval(TimeIntervalRate eEveryNMinutes)
        {
            int iSeconds = 60;
            switch (eEveryNMinutes)
            {
                case TimeIntervalRate.EveryMinute:
                    iSeconds = 60;
                    break;
                case TimeIntervalRate.Every5Minutes:
                    iSeconds = 300;
                    break;
                case TimeIntervalRate.Every10Minutes:
                    iSeconds = 600;
                    break;
                case TimeIntervalRate.Every15Minutes:
                    iSeconds = 15 * 60;
                    break;
                case TimeIntervalRate.Every30Minutes:
                    iSeconds = 30 * 60;
                    break;
                case TimeIntervalRate.Hourly:
                    iSeconds = 60 * 60;
                    break;
            }
            return iSeconds;
        }
        #endregion

        #region IsToday
        /// <summary>
        /// Return whether this DateTime corresponds to today (according to DateTime.NOw)
        /// </summary>
        public static bool IsToday(this DateTime when)
        {
            return when.IsSameDayAs(DateTime.Now);
        }
        #endregion

        #region IsMatchForThisHour
        /// <summary>
        /// Given a list of words, return true if they refer to a time-interval equiv to "the past hour".
        /// </summary>
        /// <param name="listExpression">an IList of 0, 1, 2, or 3 words</param>
        /// <returns>true if the words represents today</returns>
        public static bool IsMatchForThisHour(IList<string> listExpression)
        {
            // Note: This is far more succinctly implemented in F# thus:
            //let IsThisHour(x : List<string>) : bool =
            //    match x with
            //        | "this"::e2::_ when IsHour e2 -> true
            //        | "this"::"past"::e3::_ when IsHour e3 -> true
            //        | "the"::"past"::e3::_ when IsHour e3 -> true
            //        | _ -> false
            //
            // However I'm trying to minimize the dependencies at this moment.
            bool isMatch = false;
            if (listExpression != null)
            {
                int n = listExpression.Count;
                if (n >= 2)
                {
                    string word1 = listExpression[0].ToLower();
                    string word2 = listExpression[1].ToLower();
                    if (n == 2)
                    {
                        if (word1.Equals("this") && (word2.Equals("hour") || word2.Equals("hr")))
                        {
                            isMatch = true;
                        }
                    }
                    else if (n == 3)
                    {
                        string word3 = listExpression[2].ToLower();
                        if ((word1.Equals("this") || word1.Equals("the")) && word2.Equals("past") && (word3.Equals("hour") || word3.Equals("hr")))
                        {
                            isMatch = true;
                        }
                    }
                }
            }
            return isMatch;
        }
        #endregion IsMatchForToday

        #region IsMatchForToday
        /// <summary>
        /// Return true if the given list of words can be loosely interpreted as meaning today (in English).
        /// </summary>
        /// <param name="listExpression">an IList of 0, 1, 2, or 3 words</param>
        /// <returns>true if the words represents today</returns>
        public static bool IsMatchForToday(IList<string> listExpression)
        {
            // Note: This is far more succinctly implemented in F# thus:
            //let IsThisToday(x : List<string>) : bool =
            //    match x with
            //    | ["today"] | ["this"; "day"] -> true
            //    | ["the"; "past"; "day"] -> true
            //    | ["since"; "yesterday"] -> true
            //    | _ -> false  
            //
            // However I'm trying to minimize the dependencies at this moment.
            bool isMatch = false;
            if (listExpression != null)
            {
                int n = listExpression.Count;
                if (n >= 1)
                {
                    string word1 = listExpression[0].ToLower();
                    if (n == 1)
                    {
                        if (word1.Equals("today"))
                        {
                            isMatch = true;
                        }
                    }
                    else
                    {
                        string word2 = listExpression[1].ToLower();
                        if (n == 2)
                        {
                            if (word1.Equals("this") && word2.Equals("day"))
                            {
                                isMatch = true;
                            }
                            else if (word1.Equals("since") && word2.Equals("yesterday"))
                            {
                                isMatch = true;
                            }
                        }
                        else if (n == 3)
                        {
                            string word3 = listExpression[2].ToLower();
                            if ((word1.Equals("this") || word1.Equals("the")) && word2.Equals("past") && word3.Equals("day"))
                            {
                                isMatch = true;
                            }
                        }
                    }
                }
            }
            return isMatch;
        }
        #endregion IsMatchForToday

        #region IsYesterday
        /// <summary>
        /// Return true if this DateTime corresponds to yesterday.
        /// </summary>
        public static bool IsYesterday(this DateTime when)
        {
            var dayAfter = when.AddDays(1);
            return dayAfter.IsSameDayAs(DateTime.Now);
        }
        #endregion

        #region IsSameDayAs
        /// <summary>
        /// Return whether this DateTime corresponds to the same day as the given DateTime.
        /// </summary>
        public static bool IsSameDayAs(this DateTime thisTime, DateTime otherTime)
        {
            return thisTime.Year == otherTime.Year && thisTime.DayOfYear == otherTime.DayOfYear;
        }
        #endregion

        #region ToStringMinimum

        // Provided for VS2008 since that does not support default parameter values.
        public static string ToStringMinimum(this DateTime t)
        {
            return ToStringMinimum(t, false);
        }

        /// <summary>
        /// Return a string representation of this DateTime, in my own concise standardized way.
        /// Year is shown only if different that this year, and month/day is included only if other than today.
        /// </summary>
        /// <param name="t">the DateTime to convert to a string</param>
        /// <param name="isToShowSeconds">whether to include seconds in the time expression</param>
        /// <returns></returns>
        public static string ToStringMinimum(this DateTime t, bool isToShowSeconds)
        {
            string sResult;
            if (t.IsToday())
            {
                if (isToShowSeconds)
                {
                    sResult = String.Format("{0:h:mm:sstt}", t);
                }
                else
                {
                    sResult = String.Format("{0:h:mmtt}", t);
                }
            }
            else if (t.Year == DateTime.Now.Year)
            {
                if (isToShowSeconds)
                {
                    sResult = String.Format("{0:M-dd h:mm:sstt}", t);
                }
                else
                {
                    sResult = String.Format(_sDateTimeFormatNoYear, t);
                }
            }
            else
            {
                if (isToShowSeconds)
                {
                    sResult = String.Format("{0:yyyy-M-dd h:mm:sstt}", t);
                }
                else
                {
                    sResult = String.Format(_sDateTimeFormat, t);
                }
            }
            // Make the AM or PM lowercase.
            sResult = sResult.Replace("AM", "am");
            sResult = sResult.Replace("PM", "pm");
            return sResult;
        }

        /// <summary>
        /// Return a string representation of the given TimeSpan,
        /// showing Hours only if that is nonzero, Minutes, Seconds, and the Milliseconds only if Hours is zero.
        /// </summary>
        /// <param name="t">The TimeSpan to render as a string</param>
        /// <returns>a string representing the given TimeSpan t</returns>
        public static string ToStringMinimum(this TimeSpan t)
        {
            var sb = new StringBuilder();
            try
            {

                if (t.Hours != 0)
                {
                    sb.Append(t.Hours.ToString());
                    sb.Append(":");
                }
                if (t.Minutes != 0)
                {
                    sb.Append(String.Format("{0:D2}", t.Minutes));
                    sb.Append(":");
                }
                sb.Append(t.Seconds);
                // Only show milliseconds if the Hours is zero.
                if (t.Hours == 0)
                {
                    if (t.Milliseconds != 0)
                    {
                        sb.Append(".");
                        sb.Append(String.Format("{0:D3}", t.Milliseconds));
                    }
                }
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception in TimeLib.ToStringMinimum(TimeSpan): " + x.Message);
            }
            return sb.ToString();
        }
        #endregion

        #region ToStringWithoutYearIfSame
        public static string ToStringWithoutYearIfSame(this DateTime thisTime)
        {
            string sResult;
            if (thisTime.Year == DateTime.Now.Year)
            {
                sResult = String.Format(_sDateTimeFormatNoYear, thisTime);
                //sResult = String.Format("{0:M-dd h:mm:SS}", thisTime);
            }
            else
            {
                sResult = String.Format(_sDateTimeFormat, thisTime);
            }
            return sResult;
        }
        #endregion

        #region DateTimeFullFormat
        /// <summary>
        /// Get the String.Format formatting-string for our date-time display (excluding seconds).
        /// </summary>
        public static string DateTimeFullFormat
        {
            get { return _sDateTimeFormat; }
        }
        #endregion

        #region AsDateString
        /// <summary>
        /// Return the given DateTime formatted as a string using format {0:yyyy-M-dd}
        /// </summary>
        public static string AsDateString(this DateTime t)
        {
            return String.Format(_sDateFormat, t);
        }
        #endregion

        #region AsDateTimeString
        /// <summary>
        /// Return the given DateTime formatted as a string using format {0:yyyy-M-dd h:mmtt}
        /// </summary>
        public static string AsDateTimeString(this DateTime t)
        {
            return String.Format(_sDateTimeFormat, t);
        }
        #endregion

        #region AsDateTimeWithoutYearString
        /// <summary>
        /// Return the given DateTime formatted as a string using format {0:M-dd h:mmtt}
        /// </summary>
        public static string AsDateTimeWithoutYearString(this DateTime t)
        {
            return String.Format(_sDateTimeFormatNoYear, t);
        }
        #endregion

        #region AddWeekdays
        public static DateTime AddWeekdays(this DateTime date, int days)
        {
            // from http://dotnetslackers.com/articles/aspnet/5-Helpful-DateTime-Extension-Methods.aspx
            var sign = days < 0 ? -1 : 1;
            var unsignedDays = Math.Abs(days);
            var weekdaysAdded = 0;
            while (weekdaysAdded < unsignedDays)
            {
                date = date.AddDays(sign);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    weekdaysAdded++;
            }
            return date;
        }
        #endregion

        #region SetTime
        public static DateTime SetTime(this DateTime date, int hour)
        {
            // from http://dotnetslackers.com/articles/aspnet/5-Helpful-DateTime-Extension-Methods.aspx
            return date.SetTime(hour, 0, 0, 0);
        }
        public static DateTime SetTime(this DateTime date, int hour, int minute)
        {
            return date.SetTime(hour, minute, 0, 0);
        }
        public static DateTime SetTime(this DateTime date, int hour, int minute, int second)
        {
            return date.SetTime(hour, minute, second, 0);
        }
        public static DateTime SetTime(this DateTime date, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, minute, second, millisecond);
        }
        #endregion

        #region FirstDayOfMonth, LastDayOfMonth
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            // from http://dotnetslackers.com/articles/aspnet/5-Helpful-DateTime-Extension-Methods.aspx
            return new DateTime(date.Year, date.Month, 1);
        }
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }
        #endregion

        #region nullable-DateTime ToString
        public static string ToString(this DateTime? date)
        {
            // from http://dotnetslackers.com/articles/aspnet/5-Helpful-DateTime-Extension-Methods.aspx
            return date.ToString(null, DateTimeFormatInfo.CurrentInfo);
        }
        public static string ToString(this DateTime? date, string format)
        {
            return date.ToString(format, DateTimeFormatInfo.CurrentInfo);
        }
        public static string ToString(this DateTime? date, IFormatProvider provider)
        {
            return date.ToString(null, provider);
        }
        public static string ToString(this DateTime? date, string format, IFormatProvider provider)
        {
            if (date.HasValue)
                return date.Value.ToString(format, provider);
            else
                return string.Empty;
        }
        #endregion

        #region ToRelativeDateString
        public static string ToRelativeDateString(this DateTime date)
        {
            // from http://dotnetslackers.com/articles/aspnet/5-Helpful-DateTime-Extension-Methods.aspx
            return GetRelativeDateValue(date, DateTime.Now);
        }
        public static string ToRelativeDateStringUtc(this DateTime date)
        {
            return GetRelativeDateValue(date, DateTime.UtcNow);
        }
        private static string GetRelativeDateValue(DateTime date, DateTime comparedTo)
        {
            TimeSpan diff = comparedTo.Subtract(date);
            if (diff.TotalDays >= 365)
                return string.Concat("on ", date.ToString("MMMM d, yyyy"));
            if (diff.TotalDays >= 7)
                return string.Concat("on ", date.ToString("MMMM d"));
            else if (diff.TotalDays > 1)
                return string.Format("{0:N0} days ago", diff.TotalDays);
            else if (diff.TotalDays == 1)
                return "yesterday";
            else if (diff.TotalHours >= 2)
                return string.Format("{0:N0} hours ago", diff.TotalHours);
            else if (diff.TotalMinutes >= 60)
                return "more than an hour ago";
            else if (diff.TotalMinutes >= 5)
                return string.Format("{0:N0} minutes ago", diff.TotalMinutes);
            if (diff.TotalMinutes >= 1)
                return "a few minutes ago";
            else
                return "less than a minute ago";
        }
        #endregion

        #region GetAvailableTimeZoneStrings
        /// <summary>
        /// Return a collection of strings representing the available local time zones to choose from.
        /// </summary>
        /// <returns>A ReadOnlyCollection of strings denoting time zones</returns>
        public static ReadOnlyCollection<string> GetAvailableTimeZoneStrings()
        {
            // This is provided just for VS2008 since that does not support default parameter values.
            return GetAvailableTimeZoneStrings(false);
        }

        /// <summary>
        /// Return a collection of strings representing the available time zones to choose from.
        /// </summary>
        /// <param name="justTheLocalOnes">Indicates whether to return only a simplified list of local zones</param>
        /// <returns>A ReadOnlyCollection of strings denoting time zones</returns>
        public static ReadOnlyCollection<string> GetAvailableTimeZoneStrings(bool justTheLocalOnes)
        {
            ReadOnlyCollection<string> availableTimeZoneStrings;
            if (justTheLocalOnes)
            {
                var list = new List<string>();
                list.Add(_sEST);
                list.Add(_sCST);
                list.Add(_sMST);
                list.Add(_sPST);
                availableTimeZoneStrings = list.AsReadOnly();
            }
            else
            {
                ReadOnlyCollection<TimeZoneInfo> timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();
                List<string> list = (from tz in timeZoneInfos select tz.Id).ToList();
                availableTimeZoneStrings = list.AsReadOnly();
            }
            return availableTimeZoneStrings;
        }
        #endregion

        #region AsString  extension method for TimeZoneInfo
        /// <summary>
        /// Given a TimeZoneInfo, return it's ToString representation unless it's a timezone for which we have
        /// our own string representation defined, in which case return that instead.
        /// </summary>
        /// <param name="timeZoneInfo">The TimeZoneInfo to return a string identification of</param>
        /// <returns>Either our simplified standard string identifier, or it's ToString result</returns>
        public static string AsString(this TimeZoneInfo timeZoneInfo)
        {
            if (timeZoneInfo.Id.Equals("Eastern Standard Time"))
            {
                return _sEST;
            }
            else if (timeZoneInfo.Id.Equals("Central Standard Time"))
            {
                return _sCST;
            }
            else if (timeZoneInfo.Id.Equals("Mountain Standard Time"))
            {
                return _sMST;
            }
            else if (timeZoneInfo.Id.Equals("Pacific Standard Time"))
            {
                return _sPST;
            }
            return timeZoneInfo.ToString();
        }
        #endregion

        #region TimeZoneInfoFromStdString
        /// <summary>
        /// Give a string that corresponds to a TimeZoneInfo Id or ToString representation,
        /// or to one of our simplified representations, return the corresponding TimeZoneInfo.
        /// </summary>
        public static TimeZoneInfo TimeZoneInfoFromStdString(string standardString)
        {
            if (standardString.Equals(_sEST))
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }
            else if (standardString.Equals(_sCST))
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            }
            else if (standardString.Equals(_sMST))
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
            }
            else if (standardString.Equals(_sPST))
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            }
            return TimeZoneInfo.FromSerializedString(standardString);
        }
        #endregion

        #region fields

        private const string _sDateFormat = "{0:yyyy-M-dd}";
        private const string _sDateTimeFormat = "{0:yyyy-M-dd h:mmtt}";
        private const string _sDateTimeFormatNoYear = "{0:M-dd h:mmtt}";
        // Use these simplified identifiers for the mainland US time zones.
        private const string _sEST = "(UTC-5) Eastern Time";
        private const string _sCST = "(UTC-6) Central Time";
        private const string _sMST = "(UTC-7) Mountain Time";
        private const string _sPST = "(UTC-8) Pacific Time";

        #endregion fields
    }
}
