namespace AdventOfCode.Solutions
{
    public abstract class BaseSolution
    {
        public BaseSolution(string path)
        {
            Lines = [.. File.ReadAllLines(path)];
            DoubleSpaced = [.. string.Join("\n", Lines).Split("\n\n")];
        }

        public List<string> Lines { get; }
        public List<string> DoubleSpaced { get; }
    }
}
