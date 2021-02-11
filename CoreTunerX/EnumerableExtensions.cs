using System;
using System.Collections.Generic;

namespace CoreTunerX
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int takeCount)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (takeCount < 0) { throw new ArgumentOutOfRangeException("takeCount", "must not be negative"); }
            if (takeCount == 0) { yield break; }

            T[] result = new T[takeCount];
            int i = 0;

            int sourceCount = 0;
            foreach (T element in source)
            {
                result[i] = element;
                i = (i + 1) % takeCount;
                sourceCount++;
            }

            if (sourceCount < takeCount)
            {
                takeCount = sourceCount;
                i = 0;
            }

            for (int j = 0; j < takeCount; ++j)
            {
                yield return result[(i + j) % takeCount];
            }
        }
    }
}
