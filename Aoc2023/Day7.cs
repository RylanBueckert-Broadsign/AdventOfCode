using AoC2023.Utils;

namespace AoC2023;

public static class Day7
{
    public static void Run()
    {
        var lines = InputHelper.ReadLines(@"Day7\input.txt");

        var hands = lines.Select(l =>
        {
            var cards = l[..5];
            var bid = int.Parse(l[6..]);

            return new Hand(cards, bid);
        });

        var orderedHands = hands
            .OrderBy(h => GetHandPower(h.Cards))
            .ThenBy(h => GetCardPower(h.Cards[0]))
            .ThenBy(h => GetCardPower(h.Cards[1]))
            .ThenBy(h => GetCardPower(h.Cards[2]))
            .ThenBy(h => GetCardPower(h.Cards[3]))
            .ThenBy(h => GetCardPower(h.Cards[4]))
            .ToList();

        var winnings = orderedHands.Select((hand, idx) => (idx + 1) * hand.Bid);

        Console.WriteLine(winnings.Sum());
    }

    private record Hand(string Cards, int Bid);

    private static int GetCardPower(char card)
    {
        const string powers = "J23456789TQKA";

        return powers.IndexOf(card);
    }

    private static int GetHandPower(string cards)
    {
        var countsDict = CountCards(cards);
        var jokers = countsDict.TryGetValue('J', out var j) ? j : 0;
        countsDict.Remove('J');

        var counts = countsDict.Select(c => c.Value).ToList();

        if (IsFiveOfAKind(counts, jokers)) return 6;
        if (IsFourOfAKind(counts, jokers)) return 5;
        if (IsFullHouse(counts, jokers)) return 4;
        if (IsThreeOfAKind(counts, jokers)) return 3;
        if (IsTwoPair(counts, jokers)) return 2;
        return IsPair(counts, jokers) ? 1 : 0;
    }

    private static Dictionary<char, int> CountCards(string cards)
    {
        var result = new Dictionary<char, int>();

        foreach (var card in cards)
        {
            if (result.ContainsKey(card))
            {
                result[card]++;
            }
            else
            {
                result[card] = 1;
            }
        }

        return result;
    }

    private static bool IsFiveOfAKind(List<int> counts, int jokers)
    {
        return jokers == 5 || counts.Any(card => card + jokers >= 5);
    }

    private static bool IsFourOfAKind(List<int> counts, int jokers)
    {
        return counts.Any(card => card + jokers >= 4);
    }

    private static bool IsFullHouse(List<int> counts, int jokers)
    {
        return counts.OrderByDescending(c => c).Take(2).Sum() + jokers >= 5;
        //
        // var most = mostOrder.FirstOrDefault();
        //
        // var secondMost = mostOrder.Count() > 1 ? mostOrder[1] : 0;
        //
        // if (most < 3)
        // {
        //     jokers -= 3 - most;
        // }
        //
        // return jokers >= 0 && secondMost + jokers >= 2;
    }

    private static bool IsThreeOfAKind(List<int> counts, int jokers)
    {
        return counts.Any(card => card + jokers >= 3);
    }

    private static bool IsTwoPair(List<int> counts, int jokers)
    {
        return counts.OrderByDescending(c => c).Take(2).Sum() + jokers >= 4;

        // var most = mostOrder.FirstOrDefault();
        //
        // var secondMost = mostOrder.Count() > 1 ? mostOrder[1] : 0;
        //
        // if (most < 2)
        // {
        //     jokers -= 2 - most;
        // }
        //
        // return jokers >= 0 && secondMost + jokers >= 2;
    }

    private static bool IsPair(List<int> counts, int jokers)
    {
        return counts.Any(card => card + jokers >= 2);
    }
}
