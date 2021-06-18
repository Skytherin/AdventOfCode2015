using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode2015.Utils
{
    public static class StructuredRx
    {
        public static T Parse<T>(string input) where T : new()
        {
            var properties = typeof(T).GetProperties().Where(p => p.SetMethod != null).ToList();

            var pattern = "";

            foreach (var property in properties)
            {
                var name = property.Name;
                var rxFormat = property.GetCustomAttribute<RxFormat>();
                if (property.PropertyType == typeof(int))
                {
                    pattern += $"(?<{name}>\\d+)";
                }
                else
                {
                    throw new NotImplementedException();
                }

                if (rxFormat?.After is { } after)
                {
                    pattern += after;
                }
            }

            pattern = $"^({pattern})$";

            var match = Regex.Match(input, pattern);

            if (!match.Success) throw new ApplicationException();

            var result = new T();

            foreach (var property in properties)
            {
                var g = match.Groups[property.Name].Value;

                if (property.PropertyType == typeof(int))
                {
                    property.SetValue(result, Convert.ToInt32(g));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return result;
        }
    }
}