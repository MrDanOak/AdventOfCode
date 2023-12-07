using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions;

public partial class Day02 : BaseSolution, ISolution
{
    public int Day => 2;

    public Day02() : base("Inputs/Day02.txt")
    { }

    private Round ParseRound(string input) 
    {
        var parts = input.Split(",").Select(x => x.Trim());
        var red = parts.FirstOrDefault(x => x.EndsWith("red"));
        var green = parts.FirstOrDefault(x => x.EndsWith("green"));
        var blue = parts.FirstOrDefault(x => x.EndsWith("blue"));
        int redVal = 0, blueVal = 0, greenVal = 0;
        if (red is not null) redVal = int.Parse(DigitRegex().Match(red).Value);
        if (blue is not null) blueVal = int.Parse(DigitRegex().Match(blue).Value);
        if (green is not null) greenVal = int.Parse(DigitRegex().Match(green).Value);
        return new Round(greenVal, redVal, blueVal);
    }

    private IEnumerable<Round> GetRounds(string input)
    {
        var roundInput = GameRegex().Replace(input, "");
        var rounds = roundInput.Trim().Split(";");
        return rounds
            .Select(ParseRound);
    }

    public int ExtractGame(string input) 
    {
        var gameMatch = GameRegex().Match(input);
        return int.Parse(gameMatch.Groups.Values.Last().Value);
    }

    public object Part1() => Lines
        .Select(x => GetRounds(x).All(x => x.Red <= 12 && x.Green <= 13 && x.Blue <= 14) ? ExtractGame(x) : 0)
        .Sum();

    public object Part2() => Lines
        .Select(x => {
            var rounds = GetRounds(x);
            var r = rounds.Max(x => x.Red);
            var g = rounds.Max(x => x.Green);
            var b = rounds.Max(x => x.Blue);
            return r * g * b;
        }).Sum();

    [GeneratedRegex("Game (\\d+): ")]
    private static partial Regex GameRegex();

    [GeneratedRegex("\\d+")]
    private static partial Regex DigitRegex();

    private record Round(int Green, int Red, int Blue);
}