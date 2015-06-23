using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace AMV.Helpers
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Mini Helper to allow iteration through dates, day by day.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="thru"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }


        /// <summary>
        /// Round the datetime to nearest timespan. 
        /// The following code is from SO: http://stackoverflow.com/q/1393696/809357
        /// </summary>
        /// <param name="date"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public static DateTime Round(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks + (span.Ticks / 2) + 1) / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }

        public static DateTime Floor(this DateTime date, TimeSpan span)
        {
            long ticks = date.Ticks / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }

        public static DateTime Ceil(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks + span.Ticks - 1) / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }



        /// <summary>
        /// Calculate Age in years based on provided DateTime
        /// </summary>
        /// <param name="dateOfBirth">Date of birth</param>
        /// <returns>Null if provided value is null, otherwise age in years</returns>
        public static int? Age(this DateTime? dateOfBirth)
        {
            if (!dateOfBirth.HasValue)
            {
                return default(int?);
            }
            return dateOfBirth.Value.Age();
        }

        /// <summary>
        /// Calculate Age in years based on provided DateTime
        /// </summary>
        /// <param name="dateOfBirth">Date of birth</param>
        /// <returns>Age in years</returns>
        public static int Age(this DateTime dateOfBirth)
        {
            var today = TimeProvider.Current.Today;
            var bday = dateOfBirth.Date;
            var age = today.Year - bday.Year;
            if (bday > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}
