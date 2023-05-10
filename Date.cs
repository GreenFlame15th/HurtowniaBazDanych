using System;
using System.Collections.Generic;
using System.Text;

namespace HurtowniaBazDanych
{
    public class Date : DataObject
    {
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        
        public Date(int year, int month, int day) : base("date")
        {
            //everything else
            this.year = year;
            this.month = month;
            this.day = day;
        }

        public override String ToString(string spacer = "")
        {
            return id + spacer + year + spacer + month + spacer + day;
        }

        public static List<Date> GenerateDatesYears(DateTime startDate, int nYears)
        {
            DateTime endDate = startDate.AddYears(nYears);
            DateTime currentDate = startDate;
            List<Date> dates = new List<Date>();
            while (currentDate <= endDate)
            {
                dates.Add(new Date(currentDate.Year, currentDate.Month, currentDate.Day));
                currentDate = currentDate.AddDays(1);
            }
            return dates;
        }
        public static List<Date> GenerateDatesMonths(DateTime startDate, int nMonths)
        {
            DateTime endDate = startDate.AddMonths(nMonths);
            DateTime currentDate = startDate;
            List<Date> dates = new List<Date>();
            while (currentDate <= endDate)
            {
                dates.Add(new Date(currentDate.Year, currentDate.Month, currentDate.Day));
                currentDate = currentDate.AddDays(1);
            }
            return dates;
        }
    }
}
