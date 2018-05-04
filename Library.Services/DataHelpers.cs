using Library.Data;
using System;
using System.Collections.Generic;

namespace Library.Services
{
    /// <summary>
    ///
    /// <summary>
    public class DataHelpers
    {
        public static IEnumerable<string> HumanizeBusinessHours(IEnumerable<BranchHours> branchHours)
        {
            var hours = new List<string>();

            foreach (var time in branchHours)
            {
                var day = HumanizeDay(time.DayOfWeek);
                var openTime = HumanizeTime(time.OpenTIme);
                var closeTime = HumanizeTime(time.CloseTime);

                var timeEntry = $"{day} {openTime} a {closeTime}";
                hours.Add(timeEntry);
            }

            return hours;
        }

        public static string HumanizeTime(int time)
        {
            return TimeSpan.FromHours(time).ToString("hh':'mm");
        }

        public static string HumanizeDay(int number)
        {
            // Database correlates 1 -> Sunday, so we substract 1
            return Enum.GetName(typeof(DayofWeekSpanish), number - 1);
        }


    }

    public enum DayofWeekSpanish
    {
        Domingo = 0,
        Lunes = 1,
        Martes = 2,
        Miércoles = 3,
        Jueves = 4,
        Viernes = 5,
        Sábado = 6
    }
}
