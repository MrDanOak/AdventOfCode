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

        public object Part1()
        {
            var onInstruction = 0;
            var location = Locations.First(x => x.Name == "AAA");
            var instructions = Lines.First();
            while (location.Name != "ZZZ")
            {
                var goLeft = instructions[onInstruction % instructions.Length] is 'L';
                location = goLeft ? location.GetLeft(Locations) : location.GetRight(Locations);
                onInstruction++;
            }
            return onInstruction;
        }

        public object Part2()
        {
            var onInstruction = 0;
            var nodes = Locations.Where(x => x.Name.EndsWith('A')).ToList();
            var instructions = Lines.First();
            while (!nodes.All(x => x.Name.EndsWith('Z')))
            {
                if (nodes.Any(x => x.Name.EndsWith('Z')))
                {
                    var endsWithZ = nodes.Where(x => x.Name.EndsWith('Z')).ToList();
                    Console.WriteLine($"[{onInstruction}] {endsWithZ.Count} locations ending with Z:");
                    endsWithZ.ForEach(Console.WriteLine);
                }

                var goLeft = instructions[onInstruction % instructions.Length] is 'L';
                var nextNodes = nodes.Select(y => goLeft ? y.Left : y.Right).ToList();
                nodes = Locations.Where(x => nextNodes.Contains(x.Name)).ToList();
                onInstruction++;
            }
            return onInstruction;
        }
    }
}
