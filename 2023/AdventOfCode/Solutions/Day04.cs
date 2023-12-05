using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{
    public class Day04 : BaseSolution, ISolution
    {
        private record Card(int Number, List<int> Left, List<int> Right)
        {
            public int Intersect() => Left.Intersect(Right).Count();
        }

        public int Day => 4;

        private List<Card> Cards;
        public Day04() : base("Inputs/Day04.txt")
        {
            Cards = Lines.Select(x =>
            {
                var parts = x.Split('|', ':');
                var rx = new Regex(@"\d+");
                return new Card(
                    int.Parse(rx.Match(parts[0]).Value),
                    rx.Matches(parts[1]).Select(x => int.Parse(x.Value)).ToList(),
                    rx.Matches(parts[2]).Select(x => int.Parse(x.Value)).ToList()
                );
            }).ToList();
        }

        private List<Card> GetCopies(List<Card> source, int cardNo)
            => source.Where(x => x.Number == cardNo)
                .Select(x => x.Intersect())
                .SelectMany(copyCount => 
                    Enumerable.Range(cardNo + 1, copyCount)
                    .Select(y => source.First(z => z.Number == y)))
                .ToList();

        public object Part1()
        => Cards
            .Select(x => x.Intersect() == 0 ? 0 : Math.Pow(2, x.Intersect() - 1))
            .Sum();

        public object Part2()
        {
            var listWithDuplicates = new List<Card>(Cards);
            var onCard = 1;
            while (listWithDuplicates.Any(x => x.Number == onCard))
            {
                listWithDuplicates.AddRange(GetCopies(listWithDuplicates, onCard));
                onCard += 1;
            }

            return listWithDuplicates.Count;
        }
    }
}
