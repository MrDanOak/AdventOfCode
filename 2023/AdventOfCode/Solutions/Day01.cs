using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions;

public class Day01 : BaseSolution, ISolution
{
    public int Day => 1;

    public Day01() : base("Inputs/Day01.txt")
    { }

    private int ExtractIntegers(string input) 
    {
        var digits = Regex.Replace(input, "[^\\d]+", "");
        return digits.Length == 0 ? 0 : int.Parse($"{digits[0]}{digits[^1]}");
    }

    private int ExtractIntegersAndWords(string input) 
    {
        var replacements = new Dictionary<string, string>
        { 
            { "one", "o1e" },
            { "two", "t2o" },
            { "three", "t3e" },
            { "four", "f4r" },
            { "five", "f5e" },
            { "six", "s6x" },
            { "seven", "s7n" },
            { "eight", "e8t" },
            { "nine", "n9e" },
        };

        foreach (var (old, replacement) in replacements)
        { 
            input = input.Replace(old, replacement);
        }
        return ExtractIntegers(input);
    }

    public object Part1() => 
        Lines
        .Select(ExtractIntegers)
        .Sum();

    public object Part2() =>
        Lines
        .Select(ExtractIntegersAndWords)
        .Sum();
}