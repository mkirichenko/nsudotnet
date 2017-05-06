using System;

namespace Kirichenko.Nsudotnet.Calendar
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter date with month to show:");

            var oldBackgroundColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Blue;
            var dateString = Console.ReadLine();
            Console.BackgroundColor = oldBackgroundColor;

            DateTime date;
            if (DateTime.TryParse(dateString, out date))
            {
                var workdays = 0;
                PrintDaysOfWeekHeader();
                PrintFirstDayOffset(date.Year, date.Month);
                for (var i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); ++i)
                {
                    var monthDay = new DateTime(date.Year, date.Month, i);
                    if (IsWorkday(monthDay.DayOfWeek))
                    {
                        ++workdays;
                    }
                    PrintMonthDay(monthDay);
                }
                Console.WriteLine();
                Console.WriteLine("Workdays in this month: {0}.", workdays);
            }
            else
            {
                Console.WriteLine("Invalid date format");
            }
        }

        private static void PrintDaysOfWeekHeader()
        {
            var oldForegroundColor = Console.ForegroundColor;
            Console.Write("MO TU WE TH FR ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("SA SU");
            Console.ForegroundColor = oldForegroundColor;
            Console.WriteLine();
        }

        private static void PrintFirstDayOffset(int year, int month)
        {
            var firstDay = new DateTime(year, month, 1);
            for (var i = 0; i < ((int)firstDay.DayOfWeek + 6) % 7; ++i)
            {
                Console.Write("   ");
            }
        }

        private static void PrintMonthDay(DateTime monthDay)
        {
            var oldBackgroundColor = Console.BackgroundColor;
            var oldForegroundColor = Console.ForegroundColor;
            if (monthDay.Date == DateTime.Today)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
            }
            if (!IsWorkday(monthDay.DayOfWeek))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write("{0,2}", monthDay.Day);
            Console.BackgroundColor = oldBackgroundColor;
            Console.ForegroundColor = oldForegroundColor;
            Console.Write(" ");
            if (monthDay.DayOfWeek == DayOfWeek.Sunday)
            {
                Console.WriteLine();
            }
        }

        private static bool IsWorkday(DayOfWeek dayOfWeek)
        {
            return !(dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday);
        }
    }
}