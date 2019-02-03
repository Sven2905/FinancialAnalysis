using System;

namespace Utilities
{
    public static class Day
    {
        public static bool IsWeekendHoliday(DateTime date, EnumFederalState enumFederalState,
            bool includeSaturday = true)
        {
            if (IsWeekend(date, includeSaturday)) return true;

            if (IsHoliday(date, enumFederalState)) return true;

            return false;
        }

        public static bool IsWeekend(DateTime date, bool includeSaturday = true)
        {
            var result = date.DayOfWeek == DayOfWeek.Sunday;

            if (!includeSaturday) return result;

            if (date.DayOfWeek == DayOfWeek.Saturday) result = true;

            return result;
        }

        public static bool IsHoliday(DateTime date, EnumFederalState enumFederalState)
        {
            var easterSunday = GetEasterSunday(date.Year);

            // Neujahr
            if (date.Day == 1 && date.Month == 1) return true;

            // Heilige Drei Könige
            if (enumFederalState == EnumFederalState.BW || enumFederalState == EnumFederalState.BY ||
                enumFederalState == EnumFederalState.ST)
                if (date.Day == 6 && date.Month == 1)
                    return true;

            // Karfreitag
            if (date == easterSunday.AddDays(-2)) return true;

            // Ostersonntag
            if (date == easterSunday) return true;

            // Ostermontag
            if (date == easterSunday.AddDays(1)) return true;

            // Tag der Arbeit
            if (date.Day == 1 && date.Month == 5) return true;

            // Christi Himmelfahrt
            if (date == easterSunday.AddDays(1)) return true;

            // Pfingstsonntag
            if (date == easterSunday.AddDays(49))
                if (enumFederalState == EnumFederalState.BB)
                    return true;

            // Pfingstmontag
            if (date == easterSunday.AddDays(50)) return true;

            // Frohnleichnam
            if (date == easterSunday.AddDays(60)) return true;

            // Mariä Himmelfahrt
            if (enumFederalState == EnumFederalState.BY || enumFederalState == EnumFederalState.SL)
                if (date.Day == 18 && date.Month == 8)
                    return true;

            // Tag der deutschen Einheit
            if (date.Day == 3 && date.Month == 10) return true;

            // Reformationstag
            if (enumFederalState == EnumFederalState.BB || enumFederalState == EnumFederalState.MV ||
                enumFederalState == EnumFederalState.SN || enumFederalState == EnumFederalState.ST ||
                enumFederalState == EnumFederalState.TH)
                if (date.Day == 6 && date.Month == 1)
                    return true;

            // Allerheiligen
            if (enumFederalState == EnumFederalState.BW || enumFederalState == EnumFederalState.BY ||
                enumFederalState == EnumFederalState.NW || enumFederalState == EnumFederalState.RP ||
                enumFederalState == EnumFederalState.SL)
                if (date.Day == 1 && date.Month == 11)
                    return true;

            // Buß- und Bettag
            if (enumFederalState == EnumFederalState.SN)
                if (date == GetBussUndBettag(date.Year))
                    return true;

            // 1. Weihnachtstag
            if (date.Day == 25 && date.Month == 12) return true;

            // 2. Weihnachtstag
            if (date.Day == 26 && date.Month == 12) return true;

            return false;
        }

        /// <summary>
        ///     Calculate Easter Sunday
        /// </summary>
        /// <param name="int">Year</param>
        /// <returns>Calculated date of Easter Sunday</returns>
        private static DateTime GetEasterSunday(int year)
        {
            var x = year; // das Jahr
            int k; // die Säkularzahl
            int m; // die säkulare Mondschaltung
            int s; // die säkulare Sonnenschaltung
            int a; // der Mondparameter
            int d; // der Keim für den ersten Vollmond im Frühling
            int r; // die kalendarische Korrekturgröße
            int og; // die Ostergrenze
            int sz; // der ersten Sonntag im März
            int oe; // die Entfernung des Ostersonntags von der Ostergrenze
            int os; // das Datum des Ostersonntags als Märzdatum (32.März = 1.April)
            int day;
            int month;

            k = x / 100;
            m = 15 + (3 * k + 3) / 4 - (8 * k + 13) / 25;
            s = 2 - (3 * k + 3) / 4;
            a = x % 19;
            d = (19 * a + m) % 30;
            r = (d + a / 11) / 29;
            og = 21 + d - r;
            sz = 7 - (x + x / 4 + s) % 7;
            oe = 7 - (og - sz) % 7;
            os = og + oe;

            month = 2 + (os + 30) / 31;
            day = os - 31 * (month / 4);

            return new DateTime(year, month, day);
        }

        /// <summary>
        ///     Calculate Buß- und Bettag
        /// </summary>
        /// <param name="year">Year</param>
        /// <returns>Date of Buß- und Bettag</returns>
        private static DateTime GetBussUndBettag(int year)
        {
            //	Der Buß- und Bettag ist immer ein Mittwoch, er liegt zwischen dem 16. und 22. November
            return GetLastWeekday(new DateTime(year, 11, 22), DayOfWeek.Wednesday);
        }

        private static DateTime GetLastWeekday(DateTime startDate, DayOfWeek targetDayOfWeek)
        {
            var startDayOfWeek = startDate.DayOfWeek;
            if (startDayOfWeek == targetDayOfWeek) return startDate;

            var diff = 0;
            if (startDayOfWeek < targetDayOfWeek)
                diff = targetDayOfWeek - startDayOfWeek - 7;
            else if (startDayOfWeek > targetDayOfWeek) diff = targetDayOfWeek - startDayOfWeek;

            return startDate.AddDays(diff);
        }
    }
}