using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            var groupToSetAction = new Dictionary<string, Action<string>>();

            var enumMap = new Dictionary<string, Dictionary<string, int>>();

            var result = new T();

            foreach (var property in properties)
            {
                var name = property.Name;
                var rxFormat = property.GetCustomAttribute<RxFormat>();
                if (property.PropertyType == typeof(int))
                {
                    pattern += $"(?<{name}>\\d+)";
                    groupToSetAction[property.Name] = g => property.SetValue(result, Convert.ToInt32(g));
                }
                else if (property.PropertyType.IsEnum)
                {
                    var mi = typeof(StructuredRx).GetMethod("GetEnumMap", BindingFlags.NonPublic | BindingFlags.Static);
                    var fooRef = mi!.MakeGenericMethod(property.PropertyType);
                    var map = (Dictionary<string, int>) fooRef.Invoke(null, null)!;

                    var alternation = map.Keys.Select(it => $"({it})").Join("|");
                    pattern += $"(?<{name}>{alternation})";

                    groupToSetAction[property.Name] = g => property.SetValue(result, map[g.ToLower()]);
                }
                else
                {
                    throw new NotImplementedException();
                }

                pattern += @"\s*";

                if (rxFormat?.After is { } after)
                {
                    pattern += after;
                    pattern += @"\s*";
                }
            }

            pattern = $"^({pattern})$";

            var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);

            if (!match.Success) throw new ApplicationException();

            foreach (var property in properties)
            {
                var g = match.Groups[property.Name].Value;
                groupToSetAction[property.Name](g);
            }

            return result;
        }

        private static Dictionary<string, int> GetEnumMap<T>()
        {
            var enumValues = typeof(T).GetEnumValues();
            var result = new Dictionary<string, int>();

            foreach (T value in enumValues)
            {
                var memberInfo = typeof(T)
                    .GetMember(value.ToString())
                    .First();

                if (memberInfo.GetCustomAttribute<DescriptionAttribute>() is { } description)
                {
                    result[description.Description.ToLower()] = Convert.ToInt32(value);
                }
                else
                {
                    result[memberInfo.Name.ToLower()] = Convert.ToInt32(value);
                }
            }

            return result;
        }
    }
}