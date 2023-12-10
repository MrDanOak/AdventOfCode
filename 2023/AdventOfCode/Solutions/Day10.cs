using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
    [RunSolution]
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

            public bool ConnectsTo(Pipe otherPipe, Direction? excluding) => 
                ((IsAnimal || (excluding != Direction.Left && Connectors.Contains(Direction.Left))) && (otherPipe.Connectors.Contains(Direction.Right) || otherPipe.IsAnimal) && X == otherPipe.X + 1 && Y == otherPipe.Y) ||
                ((IsAnimal || (excluding != Direction.Right && Connectors.Contains(Direction.Right))) && (otherPipe.Connectors.Contains(Direction.Left) || otherPipe.IsAnimal) && X == otherPipe.X - 1 && Y == otherPipe.Y) || 
                ((IsAnimal || (excluding != Direction.Down && Connectors.Contains(Direction.Down))) && (otherPipe.Connectors.Contains(Direction.Up) || otherPipe.IsAnimal) && Y == otherPipe.Y - 1 && X == otherPipe.X) ||
                ((IsAnimal || (excluding != Direction.Up && Connectors.Contains(Direction.Up))) && (otherPipe.Connectors.Contains(Direction.Down) || otherPipe.IsAnimal) && Y == otherPipe.Y + 1 && X == otherPipe.X);

            public Direction? DirectionFrom(Pipe otherPipe)
            {
                if (otherPipe == this) return null;
                var direction = otherPipe.X < X ? Direction.Left : 
                    otherPipe.X > X ? Direction.Right :
                    otherPipe.Y < Y ? Direction.Up :
                    otherPipe.Y > Y ? Direction.Down : 
                    (Direction?)null;

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
            var startPath = new Path(_pipes.Where(x => x.IsAnimal).ToList());
            var allPaths = new List<Path> { startPath };
            var pathsLoopedRoundToAnimal = new List<Path>();

            while (pathsLoopedRoundToAnimal.Count == 0 || allPaths.Count > 0)
            {
                var pathsToAdd = new List<Path>();
                var pathsToRemove = new List<Path>();
                foreach(var path in allPaths) 
                {
                    var pipe = path.Pipes.Last();
                    var connectingPipes = _pipes
                        .Where(otherPipe => pipe.ConnectsTo(otherPipe, path.CurrentDirection))
                        .ToList();

                    if (connectingPipes.Count == 0)
                    {
                        pathsToRemove.Add(path);
                    }

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
                allPaths.RemoveAll(x => pathsToRemove.Contains(x));
                allPaths.AddRange(pathsToAdd);
            }
            return Math.Round(allPaths.Min(x => x.Pipes.Count) / 2d, MidpointRounding.AwayFromZero);
        }

        public object Part2() => "";
    }
}
