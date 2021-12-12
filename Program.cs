using System;
using System.Linq;
using System.Reflection;
using AdventOfCode2015.Utils;

namespace AdventOfCode2015
{
    class Program
    {
        static void Main()
        {
            var regression = false;
            var dayOverride = 20;

            var days = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Select(type => (type, StructuredRx.ParseOrDefault<DayClass>(type.Name)))
                .Where(it => it.Item2 != null)
                .Select(it => new {Type = it.Item1, it.Item2!.DayNumber})
                .OrderBy(it => it.DayNumber)
                .ToList();

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (dayOverride != 0)
            {
                days = days.Where(it => it.DayNumber == dayOverride).ToList();
            }

            if (!regression)
            {
                days = days.Skip(days.Count - 1).ToList();
            }

            foreach (var day in days)
            {
                var start = DateTime.Now;
                Console.Write(day.Type.Name);
                var run = day.Type.GetMethod("Run", BindingFlags.Static | BindingFlags.Public) ?? throw new ApplicationException();
                run.Invoke(null, new object?[]{});
                var stop = DateTime.Now;
                Console.WriteLine($"  {(stop - start).TotalSeconds:N3}s");
            }
        }
    }

    public class DayClass
    {
        [RxFormat(Before = "Day")]
        public int DayNumber { get; set; }
    }
}
