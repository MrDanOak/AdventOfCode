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

            public IEnumerable<Pipe> ConnectingPipes(List<Pipe> pipes) => 
                pipes.Where(pipe => 
                    pipe.X == X - 1 && pipe.Y == Y && pipe.Connectors.Any(connector => connector == Direction.Right) ||
                    pipe.X == X + 1 && pipe.Y == Y && pipe.Connectors.Any(connector => connector == Direction.Left) ||
                    pipe.Y == Y - 1 && pipe.X == X && pipe.Connectors.Any(connector => connector == Direction.Down) || 
                    pipe.Y == Y + 1 && pipe.X == X && pipe.Connectors.Any(connector => connector == Direction.Up)
                );

            public bool IsAnimal = SourceChar == 's';
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
            var startPath = _pipes.Where(x => x.IsAnimal).ToList();
            var allPaths = new List<List<Pipe>>();
            allPaths.Add(startPath);
            while(!allPaths.Any(x => x.Count(y => y.IsAnimal) == 2))
            {
                var pathsToAdd = new List<List<Pipe>>();
                var pathsToRemove = new List<List<Pipe>>();
                foreach(var path in allPaths) 
                {
                    var pipe = path.Last();
                    var connectors = pipe.ConnectingPipes(_pipes).Where(connector => connector.IsAnimal || !path.Contains(connector)).ToList();

                    if (connectors.Count == 0)
                    {
                        pathsToRemove.Add(path);
                    }

                    for (var i = connectors.Count - 1; i >= 0; i--) 
                    {
                        if (i == 0) path.Add(connectors[i]);
                        else 
                        {
                            var newPath = new List<Pipe>(path);
                            newPath.Add(connectors[i]);
                            pathsToAdd.Add(newPath);
                        }
                    }
                }
                allPaths.Remove(pathsToRemove);
                allPaths.Add(pathsToAdd);
            }

            return 0;
        }

        public object Part2() => "";
    }
}
