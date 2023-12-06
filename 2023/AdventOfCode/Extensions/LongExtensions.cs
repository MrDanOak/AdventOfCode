using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Extensions
{
    public static class LongExtensions
    {
        public static IEnumerable<long> Range(this long start, long count)
        {
            var limit = start + count;

            while (start < limit)
            {
                yield return start;
                start++;
            }
        }
    }
}
