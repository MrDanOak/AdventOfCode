namespace AdventOfCode.Solutions
{
    public interface ISolution
    {
        public int Day { get; }
        public object Part1();
        public object Part2();
    }
}
