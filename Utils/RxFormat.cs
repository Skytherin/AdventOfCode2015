using System;

namespace AdventOfCode2015.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RxFormat : Attribute
    {
        public string? After { get; set; }

        public string? Before { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RxAlternate : Attribute
    {
    }
}