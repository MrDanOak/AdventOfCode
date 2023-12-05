using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    [RunSolution]
    public class Day05 : BaseSolution, ISolution
    {
        public int Day => 5;
        private List<Map> _transformations;
        private List<long> _seeds;
        private Regex _digitRx = new(@"\d+");
        private ConcurrentDictionary<long, long> _cache = new ConcurrentDictionary<long, long>();
        private ProgressReporter _processedRecords = new ProgressReporter(0);
        private record TransformationRange(long DestinationRangeStart, long SourceRangeStart, long RangeLength)
        {
            public bool CanTransform(long input) => input >= SourceRangeStart && input <= SourceRangeStart + RangeLength;
            public long Transform(long input) => input - SourceRangeStart + DestinationRangeStart;
        }

        private record Map(string From, string To, List<TransformationRange> TransformationRanges)
        {
            public override string ToString() => $"{From} {To} ({TransformationRanges.Count} ranges)";
        }

        public Day05() : base("Inputs/Day05.txt")
        {
            _seeds = _digitRx.Matches(Lines.First()).Select(x => long.Parse(x.Value)).ToList();
            _transformations = DoubleSpaced[1..]
                .Select(ParseMap)
                .ToList();
        }

        private Map ParseMap(string mapText)
        {
            var mapTypeRx = new Regex(@"(\w+)-to-(\w+)");
            var parts = mapText.Split("\n");
            var mapTypes = mapTypeRx.Match(parts.First());
            var transformationRanges = parts[1..].Select(y => {
                var rangeValues = _digitRx.Matches(y).Select(z => long.Parse(z.Value)).ToList();
                return new TransformationRange(rangeValues[0], rangeValues[1], rangeValues[2]);
            }).ToList();
            return new Map(mapTypes.Groups[1].Value, mapTypes.Groups[2].Value, transformationRanges);
        }

        private async Task<long> MinimumLocation(List<long> seeds)
        {
            _processedRecords = new ProgressReporter(seeds.Count, (count) =>
            {
                var percentage = (int)(((double)count / (double)seeds.Count) * 100d);
                Console.WriteLine($"Processed {count} records out of {seeds.Count} ({percentage})%");
            });

            var tasks = seeds.Select(seed =>
            {
                return Task.Run(() =>
                {
                    var cacheKey = seed;
                    if (_cache.ContainsKey(cacheKey))
                        return _cache[cacheKey];

                    _transformations.ForEach(transformation =>
                    {
                        var transformationRange = transformation
                            .TransformationRanges
                            .FirstOrDefault(x => x.CanTransform(seed));

                        if (transformationRange is not null)
                            seed = transformationRange.Transform(seed);
                    });
                    if (!_cache.ContainsKey(cacheKey))
                        _cache.TryAdd(cacheKey, seed);
                    _processedRecords.Report();
                    return seed;
                });
            });

            return (await Task.WhenAll(tasks)).Min();
        }

        public object Part1() =>
            MinimumLocation(_seeds)
            .GetAwaiter()
            .GetResult();

        //public object Part2() => MinimumLocation(_seeds.Chunk(2).SelectMany(x => CreateRange(x.First(), x.Last())).ToList());
        public object Part2() => MinimumLocation(_seeds.Chunk(2).OrderBy(x => x[0]).SelectMany(x => CreateRange(x.First(), x.Last())).ToList())
            .GetAwaiter()
            .GetResult();
        
        private static IEnumerable<long> CreateRange(long start, long count)
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
