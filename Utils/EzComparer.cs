using System;
using System.Collections.Generic;

namespace AdventOfCode2015.Utils
{
    public class EzComparer<T>: IComparer<T>
    {
        private readonly Func<T, T, int> F;

        public EzComparer(Func<T, T, int> f)
        {
            F = f;
        }

        public int Compare(T? x, T? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return F(x, y);
        }
    }
}