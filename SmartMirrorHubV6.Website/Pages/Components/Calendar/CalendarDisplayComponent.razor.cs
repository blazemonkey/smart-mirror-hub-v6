using SmartMirrorHubV6.Shared.Components.Data.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMirrorHubV6.Website.Pages.Components.Calendar
{
    public partial class CalendarDisplayComponent : MirrorGenericBaseComponent<CalendarDisplayResponse>
    {
        public override string ComponentAuthor => "Life";
        public override string ComponentName => "Calendar Display";
        private Dictionary<DayOfWeek, string> Days { get; set; }
        private CalendarItem[,] Items { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (Response == null)
                return;

            var days = new Dictionary<DayOfWeek, string>()
            {
                { DayOfWeek.Sunday, DayOfWeek.Sunday.ToString().Substring(0, 2) },
                { DayOfWeek.Monday, DayOfWeek.Monday.ToString().Substring(0, 2) },
                { DayOfWeek.Tuesday, DayOfWeek.Tuesday.ToString().Substring(0, 2) },
                { DayOfWeek.Wednesday, DayOfWeek.Wednesday.ToString().Substring(0, 2) },
                { DayOfWeek.Thursday, DayOfWeek.Thursday.ToString().Substring(0, 2) },
                { DayOfWeek.Friday, DayOfWeek.Friday.ToString().Substring(0, 2) },
                { DayOfWeek.Saturday, DayOfWeek.Saturday.ToString().Substring(0, 2) }
            };

            Days = days.SkipWhile(x => x.Key != (DayOfWeek)Response.DayToStart)
                       .Concat(days.TakeWhile(x => x.Key != (DayOfWeek)Response.DayToStart))
                       .ToDictionary(x => x.Key, y => y.Value);

            var firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            var lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, daysInMonth);

            var keys = Days.Keys.ToList();

            Items = new CalendarItem[6,7];
            var line = 0;
            var day = firstDay;
            for (var i = 0; i < daysInMonth; i++)
            {
                Items[line, keys.IndexOf(day.DayOfWeek)] = new CalendarItem() { Date = day, Day = day.Day };
                if (day.DayOfWeek == keys.LastOrDefault())                
                    line++;            

                day = day.AddDays(1);
            }

            // fill in the previous month dates
            var daysInPreviousMonth = DateTime.DaysInMonth(DateTime.Now.Month == 1 ? DateTime.Now.Year - 1 : DateTime.Now.Year, DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1);
            var previousMonthDay = new DateTime(DateTime.Now.Month == 1 ? DateTime.Now.Year - 1 : DateTime.Now.Year, DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1, daysInPreviousMonth);
            for (var i = 6; i >= 0; i--)
            {
                if (Items[0, i] != null)
                    continue;

                Items[0, i] = new CalendarItem() { Date = previousMonthDay, Day = previousMonthDay.Day };
                previousMonthDay = previousMonthDay.AddDays(-1);
            }

            // fill in the next month dates            
            var nextMonthDay = new DateTime(DateTime.Now.Month == 12 ? DateTime.Now.Year + 1 : DateTime.Now.Year, DateTime.Now.Month == 12 ? 1 : DateTime.Now.Month + 1, 1);
            var index = 0;
            while (line <= Items.GetUpperBound(0))
            {
                if (Items[line, index] != null)
                {
                    index++;
                    continue;
                }

                Items[line, index] = new CalendarItem() { Date = nextMonthDay, Day = nextMonthDay.Day };
                if (index == Items.GetUpperBound(1))
                {
                    index = 0;
                    line++;
                }
                else
                    index++;
                nextMonthDay = nextMonthDay.AddDays(1);
            }
        }
    }

    public class CalendarItem
    {
        public DateTime Date { get; set; }
        public int Day { get; set; }
        public string Text { get; set; }
    }
}
