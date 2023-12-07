using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions;

public partial class Day03 : BaseSolution, ISolution
{
    public int Day => 3;

    public Day03() : base("Inputs/Day03.txt")
    { }

    private IEnumerable<Part> GetParts(string[] lines, Regex rx) => 
        lines
        .SelectMany((line, y) => 
            rx
            .Matches(line)
            .Select(match => new Part(match.Index, y, match.Value))
        );

    public object Part1() 
    {
        var parts = GetParts([.. Lines], new Regex(@"\d+")).ToList();
        var symbols = GetParts([.. Lines], new Regex(@"[^.0-9]")).ToList();
        return parts
            .Where(part => symbols.Any(s => s.IsNeighbour(part)))
            .Select(part => part.PartNo())
            .Sum();
    }

    public object Part2()
    {
        var parts = GetParts([.. Lines], new Regex(@"\d+")).ToList();
        var gears = GetParts([.. Lines], new Regex(@"\*")).ToList();
        return gears
            .Select(gear => { 
                var neighbours = parts.Where(p => p.IsNeighbour(gear)).ToList();
                return neighbours.Count == 2 
                    ? neighbours.First().PartNo() * neighbours.Last().PartNo()
                    : 0;
            }).Sum();
    }

    private record Part(int X, int Y, string Text)
    {
        public int PartNo() => int.Parse(Text);
        public override string ToString() => $"X: {X} Y: {Y} V: {Text}";
        public bool IsNeighbour(Part other) => 
            Math.Abs(other.Y - Y) <= 1
            && X <= other.X + other.Text.Length 
            && other.X <= X + Text.Length;
    }
}