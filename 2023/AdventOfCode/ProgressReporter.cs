using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class ProgressReporter : Progress<long>, IProgress<long>
    {
        public long TotalCount { get; }
        private long _total { get; set; }
        public ProgressReporter(long totalCount) : base() { TotalCount = totalCount; }
        public ProgressReporter(long totalCount, Action<long> handler) : base(handler) { TotalCount = totalCount; }

        public void Report()
        {
            _total++;
            base.OnReport(_total);
        }
    }
}
