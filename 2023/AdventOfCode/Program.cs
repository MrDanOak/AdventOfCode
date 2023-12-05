
using AdventOfCode.Solutions;
using System.Reflection;

var solutions = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(x => 
        typeof(ISolution).IsAssignableFrom(x) 
        && x.IsClass 
        && x.GetCustomAttributes<RunSolutionAttribute>().Any())
    .Select(x => Activator.CreateInstance(x) as ISolution)
    .Cast<ISolution>()
    .ToList();

foreach (var solution in solutions)
{
    Console.WriteLine($"Day {solution.Day} Part 1 {solution.Part1()}");
    Console.WriteLine($"Day {solution.Day} Part 2 {solution.Part2()}");
}