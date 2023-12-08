namespace AdventOfCode.Solutions;

public class Day07 : BaseSolution, ISolution
{
    enum HandType
    {
        HighCard = 1,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    record Card(char CharCard)
    {
        public long Value(bool isWild = false) => CharCard switch
        {
            'T' => 10,
            'J' when !isWild => 11,
            'J' when isWild => 1,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => long.Parse(CharCard.ToString())
        };
    }

    class Hand : IComparable
    {
        protected HandType HandType { get; init; }
        public Card[] Cards { get; }
        public bool TreatJAsWild { get; }
        public long Bid { get; }

        public Hand(Card[] cards, long bid, bool treatJAsWild)
        {
            Bid = bid;
            Cards = cards;
            TreatJAsWild = treatJAsWild;
            var uniqueCards = cards.Where(x => !treatJAsWild || x.CharCard is not 'J')
                .GroupBy(x => x.Value())
                .Select(x => x.Concat(cards.Where(x => treatJAsWild && x.CharCard is 'J')))
                .ToList();

            HandType = treatJAsWild && cards.Count(x => x.CharCard is 'J') is 5 ? HandType.FiveOfAKind : uniqueCards.Count switch
            {
                1 => HandType.FiveOfAKind,
                2 => uniqueCards.Any(x => x.Count() is 4) ? HandType.FourOfAKind : HandType.FullHouse,
                3 => uniqueCards.Any(x => x.Count() is 3) ? HandType.ThreeOfAKind : HandType.TwoPair,
                4 => HandType.OnePair,
                _ => HandType.HighCard
            };
        }

        public int CompareTo(object? obj)
        {
            if (obj is not Hand otherHand)
                throw new Exception("Invalid comparison");

            if (HandType != otherHand.HandType)
                return ((int)otherHand.HandType).CompareTo((int)HandType);

            for (var i = 0; i < Cards.Length; i++)
            {
                if (Cards[i].Value(TreatJAsWild) == otherHand.Cards[i].Value(TreatJAsWild))
                    continue;

                return otherHand.Cards[i].Value(TreatJAsWild)
                    .CompareTo(Cards[i].Value(TreatJAsWild));
            }

            throw new Exception("Should not have duplicates");
        }

        public override string ToString()
        {
            return $"{string.Join(", ", Cards.Select(x => x.CharCard))} - {HandType}";
        }
    }

    public Day07() : base("Inputs/Day07.txt")
    { }

    public int Day => 7;

    private long SortAndGetSum(List<Hand> hands)
    {
        var sorted = hands
            .OrderByDescending(x => x)
            .ToList();

        return sorted
            .Select((x, i) => x.Bid * (i + 1))
            .Sum();
    }

    public object Part1() => SortAndGetSum(Lines.Select(line =>
    {
        var parts = line.Split(' ');
        return new Hand(parts.First().Select(x => new Card(x)).ToArray(), int.Parse(parts[1]), false);
    }).ToList());

    public object Part2() => SortAndGetSum(Lines.Select(line =>
    {
        var parts = line.Split(' ');
        return new Hand(parts.First().Select(x => new Card(x)).ToArray(), int.Parse(parts[1]), true);
    }).ToList());
}
