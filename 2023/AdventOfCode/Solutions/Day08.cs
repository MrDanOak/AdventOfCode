using System;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    [RunSolution]
    public class Day08 : BaseSolution, ISolution
    {
        public List<Location> Locations { get; }
        public record Location(string Name, string Left, string Right)
        {
            public Location GetLeft(List<Location> locations) => locations.First(x => x.Name == Left);
            public Location GetRight(List<Location> locations) => locations.First(x => x.Name == Right);
            public override string ToString() => $"Name: {Name} Left: {Left} Right: {Right}";
        }

        public Day08() : base("Inputs/Day08.txt")
        {
            var charRegex = new Regex(@"[A-Z]+");
            Locations = Lines.Skip(2).Select(x =>
            {
                var parts = charRegex.Matches(x).Select(x => x.Value).ToArray();
                return new Location(parts[0], parts[1], parts[2]);
            }).ToList();
        }

        public int Day => 8;

        private long PathLength(Location startNode, Func<Location, bool> continueCondition) 
        {
            var onInstruction = 0;
            var location = startNode;
            var instructions = Lines.First();
            while (continueCondition(location))
            {
                var goLeft = instructions[onInstruction % instructions.Length] is 'L';
                location = goLeft ? location.GetLeft(Locations) : location.GetRight(Locations);
                onInstruction++;
            }
            return onInstruction;
        }

        static long GreatestCommonDivisor(long a, long b)
        {
            long remainder;
            while (b != 0)
            {
                remainder = a % b;
                a = b;
                b = remainder;
            }
            return a;
        }

        public object Part1() => PathLength(Locations.First(x => x.Name == "AAA"), (location) => location.Name != "ZZZ");

        public object Part2() => Locations.Where(x => x.Name.EndsWith("A"))
            .Select(x => PathLength(x, (location) => !location.Name.EndsWith("Z")))
            .Aggregate((a, b) => a * b / GreatestCommonDivisor(a, b));
    }
}
