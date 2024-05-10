namespace AdventOfCode.Solutions;

public class Day10 : BaseSolution, ISolution
{
    enum Direction
    {
        Left, 
        Up, 
        Right, 
        Down
    }

    record Path(List<Pipe> Pipes)
    {
        public Direction? CurrentDirection { get; set; } = null;
    }

    record Pipe(char SourceChar, int X, int Y) 
    {
        public Direction[] Connectors = SourceChar switch 
        {
            'J' => [Direction.Left, Direction.Up],
            '|' => [Direction.Down, Direction.Up],
            '-' => [Direction.Left, Direction.Right],
            'F' => [Direction.Right, Direction.Down],
            'L' => [Direction.Right, Direction.Up],
            '7' => [Direction.Down, Direction.Left],
            _ => []
        };

        public bool ConnectsTo(Pipe otherPipe, Direction? excluding = null) => 
            ((IsAnimal || (excluding != Direction.Left && Connectors.Contains(Direction.Left))) && (otherPipe.Connectors.Contains(Direction.Right) || otherPipe.IsAnimal) && X == otherPipe.X + 1 && Y == otherPipe.Y) ||
            ((IsAnimal || (excluding != Direction.Right && Connectors.Contains(Direction.Right))) && (otherPipe.Connectors.Contains(Direction.Left) || otherPipe.IsAnimal) && X == otherPipe.X - 1 && Y == otherPipe.Y) || 
            ((IsAnimal || (excluding != Direction.Down && Connectors.Contains(Direction.Down))) && (otherPipe.Connectors.Contains(Direction.Up) || otherPipe.IsAnimal) && Y == otherPipe.Y - 1 && X == otherPipe.X) ||
            ((IsAnimal || (excluding != Direction.Up && Connectors.Contains(Direction.Up))) && (otherPipe.Connectors.Contains(Direction.Down) || otherPipe.IsAnimal) && Y == otherPipe.Y + 1 && X == otherPipe.X);

        public Direction? DirectionFrom(Pipe otherPipe)
        {
            if (otherPipe == this) return null;
            var direction = otherPipe switch {
                { } p when p.X > X => Direction.Right,
                { } p when p.X < X => Direction.Left,
                { } p when p.Y < Y => Direction.Up,
                { } p when p.Y > Y => Direction.Down,
                _ => (Direction?) null
            };

            return direction;
        }

        public bool IsAnimal = SourceChar == 'S';

        public override string ToString() => $"X: {X} Y: {Y} Char: {SourceChar} {(Connectors.Length != 0 ? $"Connectors: {string.Join(", ", Connectors)}" : "")}";
    }

    private List<Pipe> _pipes;

    public Day10() : base("Inputs/Day10.txt")
    {
        _pipes = Lines.SelectMany((line, y) => 
            line
                .Select((character, x) => new Pipe(character, x, y))
                .Where(x => x.SourceChar != '.')
        ).ToList();
    }

    public int Day => 10;

    public object Part1() 
    {
        var animal = _pipes.First(x => x.IsAnimal);
        var startPath = new Path(new List<Pipe>(){ animal });
        var allPaths = new List<Path> { startPath };
        var pathsLoopedRoundToAnimal = new List<Path>();

        while (allPaths.Count(x => x.Pipes.Where(pipe => pipe.ConnectsTo(animal)).Count() == 2) == 0)
        {
            var pathsToAdd = new List<Path>();
            foreach(var path in allPaths) 
            { 
                var pipe = path.Pipes.Last();
                var connectingPipes = _pipes
                    .Where(otherPipe => pipe.ConnectsTo(otherPipe, path.CurrentDirection))
                    .ToList();

                for (var i = connectingPipes.Count - 1; i >= 0; i--) 
                {
                    if (i == 0)
                    {
                        path.Pipes.Add(connectingPipes[i]);
                        path.CurrentDirection = connectingPipes[i].DirectionFrom(pipe);
                    }
                    else
                    {
                        var newPath = new Path(new List<Pipe>(path.Pipes) { connectingPipes[i] })
                        {
                            CurrentDirection = connectingPipes[i].DirectionFrom(pipe)
                        };
                        pathsToAdd.Add(newPath);
                    }
                }
            }
            allPaths.AddRange(pathsToAdd);
        }

        var connectingToAnimals = allPaths.First(x => x.Pipes.Where(pipe => pipe.ConnectsTo(animal)).Count() == 2);

        return Math.Round(connectingToAnimals.Pipes.Count() / 2d, MidpointRounding.AwayFromZero);
    }

    public object Part2() => "";
}