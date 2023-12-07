using System.Reflection.Metadata;

namespace AdventOfCode.Solutions;

[RunSolution]
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

    abstract record BaseHand(Card[] Cards, long Bid) : IComparable
    {
        protected HandType HandType { get; init; }
        public int CompareTo(object? obj)
        {
            if (obj is not BaseHand otherHand)
                throw new Exception("Invalid comparison");

            if (HandType != otherHand.HandType)
                return ((int)otherHand.HandType).CompareTo((int)HandType);

            for (var i = 0; i < Cards.Length; i++)
            {
                if (Cards[i].Value(this is HandWithWilds) == otherHand.Cards[i].Value(this is HandWithWilds))
                    continue;

                return otherHand.Cards[i].Value(this is HandWithWilds)
                    .CompareTo(Cards[i].Value(this is HandWithWilds));
            }

            throw new Exception("Should not have duplicates");
        }

        public override string ToString()
        {
            return $"{string.Join(", ", Cards.Select(x => x.CharCard))} - {HandType}";
        }
    }

    record Hand : BaseHand
    {
        public Hand(Card[] cards, long bid) : base(cards, bid)
        {
            var uniqueCards = cards.GroupBy(x => x.Value()).ToList();
            HandType = uniqueCards.Count switch
            {
                1 => HandType.FiveOfAKind,
                2 => uniqueCards.First().Count() switch
                {
                    4 or 1 => HandType.FourOfAKind,
                    _ => HandType.FullHouse
                },
                3 => uniqueCards.Any(x => x.Count() == 3) ? HandType.ThreeOfAKind : HandType.TwoPair,
                4 => HandType.OnePair,
                _ => HandType.HighCard
            };
        }

        public override string ToString() => base.ToString();
    }

    record HandWithWilds : BaseHand
    {
        public HandWithWilds(Card[] cards, long bid) : base(cards, bid)
        {
            var uniqueCards = cards.GroupBy(x => x.Value(true)).ToList();
            var wildCards = cards.Count(x => x.CharCard == 'J');
            HandType = uniqueCards.Count switch
            {
                1 => HandType.FiveOfAKind,
                2 when wildCards is 1 => HandType.FiveOfAKind,
                2 => uniqueCards.First().Count() switch
                {
                    4 or 1 => HandType.FourOfAKind,
                    _ => HandType.FullHouse
                },
                3 => uniqueCards.Any(x => x.Count() == 3) ? HandType.ThreeOfAKind : HandType.TwoPair,
                4 => HandType.OnePair,
                _ => HandType.HighCard
            };
        }
    }

    public Day07() : base("Inputs/Day07.txt")
    { }

    public int Day => 7;

    private long SortAndGetSum(List<BaseHand> hands)
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
            return new Hand(parts.First().Select(x => new Card(x)).ToArray(), int.Parse(parts[1])) as BaseHand;
        }).ToList());

    public object Part2() => SortAndGetSum(Lines.Select(line =>
    {
        var parts = line.Split(' ');
        return new HandWithWilds(parts.First().Select(x => new Card(x)).ToArray(), int.Parse(parts[1])) as BaseHand;
    }).ToList());
}
