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

            var days = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Select(type => (type, StructuredRx.ParseOrDefault<DayClass>(type.Name)))
                .Where(it => it.Item2 != null)
                .OrderBy(it => it.Item2!.DayNumber)
                .ToList();

            if (!regression)
            {
                days = days.Skip(days.Count - 1).ToList();
            }

            foreach (var day in days)
            {
                var start = DateTime.Now;
                Console.Write(day.type.Name);
                var run = day.type.GetMethod("Run", BindingFlags.Static | BindingFlags.Public) ?? throw new ApplicationException();
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
