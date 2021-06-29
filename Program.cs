using System;
using System.Linq;
using System.Reflection;
using AdventOfCode2015.Utils;

namespace AdventOfCode2015
{
    class Program
    {
        static void Main(string[] args)
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
                Console.WriteLine(day.type.Name);
                var run = day.type.GetMethod("Run", BindingFlags.Static | BindingFlags.Public) ?? throw new ApplicationException();
                run.Invoke(null, new object?[]{});
            }
        }
    }

    public class DayClass
    {
        [RxFormat(Before = "Day")]
        public int DayNumber { get; set; }
    }
}
