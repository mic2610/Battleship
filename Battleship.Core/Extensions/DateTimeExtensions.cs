using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsWeekDay(this DateTime date) => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
    }
}
