using System;

namespace ISDCore
{
    public enum EnumFormatDate
    { Unknown = 0, Standard, DayMonth, MonthYear, Year, FiscalYear, StandardDateTime, FullStandardDate };

    public enum EnumDateInterval
    { Years, Months, Weeks, Days, Hours, Minutes, Seconds }
    public static class DateHelper
    {
        public static string MapMonthName(int Month)
        {
            string Ret = "";
            if (Month >= 1 && Month <= 12)
            {
                //string list = (EnumLanguage.Arabic == Languages.Language) ? "يناير فبراير مارس إبريل مايو يونيو يوليو أغسطس سبتمبر أكتوبر نوفمبر ديسمبر" : "January February March April May June July August September October November December";//Resources.MapLabel("MonthList");
                string list = Resources.MapLabel["ShortMonthList"];
                string[] arr = list.Split(new Char[] { ' ' });
                if (arr is Array)
                    Ret = arr[Month - 1];
            }
            return Ret;
        }
        public static string MapWeekDay(DateTime date)
        {
            string Ret = "";
            if (NullHelper.DateTime != date)
            {
                string list = Resources.MapLabel["WeekDayList"];

                string[] arr = list.Split(new Char[] { ' ' });
                if (arr is Array)
                    Ret = arr[(int)date.DayOfWeek];
            }
            return Ret;
        }
        public static string Format(DateTime Date, EnumFormatDate DateFormat)
        {
            string Ret = Date.ToString();

            switch (DateFormat)
            {
                case EnumFormatDate.FullStandardDate:
                    Ret = string.Format("{3} {0} {1} {2}", Date.Day, MapMonthName(Date.Month), Date.Year, MapWeekDay(Date));
                    break;
                case EnumFormatDate.Standard:
                    Ret = string.Format("{0} {1} {2}", Date.Day, MapMonthName(Date.Month), Date.Year); break;

                case EnumFormatDate.DayMonth:
                    Ret = string.Format("{0} {1}", Date.Day, MapMonthName(Date.Month)); break;

                case EnumFormatDate.MonthYear:
                    Ret = string.Format("{0} {1}", MapMonthName(Date.Month), Date.Year); break;

                case EnumFormatDate.Year:
                    Ret = string.Format("{0}", Date.Year); break;

                case EnumFormatDate.FiscalYear:
                    Ret = string.Format("{0}/{1}", Date.Year - 1, Date.Year); break;

                case EnumFormatDate.StandardDateTime:
                    Ret = string.Format("{0}/{1}/{2} {3}", Date.Day, Date.Month, Date.Year, Date.ToShortTimeString()); break;

                default:
                    Ret = string.Format("{0}/{1}/{2}", Date.Day, Date.Month, Date.Year); break;
            }

            return Ret;
        }
        public static string Format(DateTime Date)
        {
            return Format(Date, EnumFormatDate.Unknown);
        }
        public static string Format(DateTime Date1, DateTime Date2)
        {
            string Ret = string.Format("{0} {1} {2}", Format(Date1), Resources.MapLabel["To"], Format(Date2));

            if (Date1.Year == Date2.Year && Date1.Month == Date2.Month && Date1.Day == Date2.Day)
                Ret = string.Format("{0}", Format(Date1, EnumFormatDate.Standard));
            else if (Date1.Year == Date2.Year && Date1.Month != Date2.Month)
                Ret = string.Format("{0} - {1}, {2}", Format(Date1, EnumFormatDate.DayMonth), Format(Date2, EnumFormatDate.DayMonth), Date1.Year);
            else if (Date1.Year == Date2.Year && Date1.Month == Date2.Month)
                Ret = string.Format("{0} - {1} {2}", Date1.Day.ToString(), Date2.Day.ToString(), Format(Date1, EnumFormatDate.MonthYear));

            return Ret;
        }
        public static int DateDiff(DateTime Date1, DateTime Date2, EnumDateInterval interval)
        {
            int Ret = 0;
            TimeSpan ts = Date2.Subtract(Date1);

            switch (interval)
            {
                case EnumDateInterval.Years:
                    Ret = Date2.Year - Date1.Year;
                    break;
                case EnumDateInterval.Months:
                    Ret = (Date2.Month - Date1.Month) + (12 * (Date2.Year - Date1.Year));
                    break;
                case EnumDateInterval.Weeks:
                    //Ret = (ts.Days / 7);
                    break;
                case EnumDateInterval.Days:
                    Ret = ts.Days;
                    break;
                case EnumDateInterval.Hours:
                    Ret = ts.Hours;
                    break;
                case EnumDateInterval.Minutes:
                    Ret = ts.Minutes;
                    break;
                case EnumDateInterval.Seconds:
                    Ret = ts.Seconds;
                    break;
                default:
                    break;
            }

            return Ret;
        }
        public static int DateDiff(DateTime Date1, DateTime Date2)
        {
            return DateDiff(Date1, Date2, EnumDateInterval.Days);
        }

        public static bool DateBetween(DateTime Date, DateTime StartDate, DateTime EndDate)
        {
            return (Date >= StartDate && Date <= EndDate);
        }

    }
}
