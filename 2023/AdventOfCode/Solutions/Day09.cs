namespace AdventOfCode.Solutions;

[RunSolution]
public class Day09 : BaseSolution, ISolution
{
    public int Day => 9;

    public Day09() : base("Inputs/Day09.txt")
    {
      
    }

    public object Part1() => Solution(x => x);

    public object Part2() => Solution(x => x.Reverse());

    private object Solution(Func<IEnumerable<int>, IEnumerable<int>> postProcess) => 
      Lines.Select(line => 
        GetNextInSequence(
          postProcess(line.Split(" ")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => Convert.ToInt32(x))
          ).ToList()
        )
      ).Sum();

    private int GetNextInSequence(List<int> sequence) 
    {
      var nextSequence = sequence
        .Select((x, i) => (x, i))
        .Where(x => x.i < sequence.Count - 1)
        .Select(x => x.x)
        .Select((x, i) => sequence[i + 1] - sequence[i])
        .ToList();

      if (!nextSequence.All(x => x == 0)) 
      {
        var nextInSequence = GetNextInSequence(nextSequence);
        return sequence.Last() + nextInSequence;
      }

      return sequence.Last() - nextSequence.Last();
    }
}
