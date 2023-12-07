using AdventOfCode.Extensions;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    public class Day06 : BaseSolution, ISolution
    {
        private readonly List<Race> _races;

        record Race(long Duration, long Record)
        {
            public long SimulateDistance(long hold)
            {
                var speed = hold;
                var timeRemaining = Duration - hold;
                return timeRemaining * speed;
            }

            public List<long> HoldValuesThatBeatRecord() => 1L.Range(Duration)
                    .Select(SimulateDistance)
                    .Where(distance => distance > Record)
                    .ToList();
        }

        public Day06() : base("Inputs/Day06.txt")
        {
            var digitRx = new Regex(@"\d+");
            var times = digitRx.Matches(Lines.First()).Select(x => int.Parse(x.Value)).ToList();
            var distances = digitRx.Matches(Lines.Last()).Select(x => int.Parse(x.Value)).ToList();
            _races = times.Select((x, i) => new Race(x, distances[i])).ToList();
        }

        public int Day => 6;

        public object Part1() => _races
            .Select(x => x.HoldValuesThatBeatRecord().Count)
            .Aggregate(1, (a, b) => a * b);

        public object Part2() => new Race(
                long.Parse(string.Join("", _races.Select(x => x.Duration))),
                long.Parse(string.Join("", _races.Select(x => x.Record))))
            .HoldValuesThatBeatRecord()
            .Count;
    }
}
