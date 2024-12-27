using Aoc2024.Common;

namespace Aoc2024;

public class Day22 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath);

        var initialSecrets = input.Select(uint.Parse).ToList();

        var monkeys = initialSecrets.Select(initial =>
        {
            var secrets = new uint[2000];
            var prices = new int[2000];
            var diffs = new int[secrets.Length - 1];
            secrets[0] = initial;

            for (var i = 1; i < 2000; i++)
            {
                secrets[i] = NextSecret(secrets[i - 1]);
                prices[i] = (int)secrets[i] % 10;
                diffs[i - 1] = prices[i] % 10 - prices[i - 1] % 10;
            }

            return (prices, diffs);
        });

        var seqPrices = monkeys.Select(s =>
        {
            var firstSeqPrices = new Dictionary<(int, int, int, int), int>();

            for (var i = 3; i < s.diffs.Length; i++)
            {
                var seq = (s.diffs[i - 3], s.diffs[i - 2], s.diffs[i - 1], s.diffs[i]);
                var seqPrice = s.prices[i + 1];

                firstSeqPrices.TryAdd(seq, seqPrice);
            }

            return firstSeqPrices;
        });

        var totalSeqPrices = seqPrices.Aggregate(
            new Dictionary<(int, int, int, int), int>(),
            (acc, next) =>
            {
                foreach (var (seq, price) in next)
                {
                    if (!acc.TryAdd(seq, price))
                    {
                        acc[seq] += price;
                    }
                }

                return acc;
            }
        );

        var bestSeq = totalSeqPrices.MaxBy(kv => kv.Value);

        Console.WriteLine(bestSeq.Value);
    }

    private static uint NextSecret(uint secret)
    {
        MixPrune(secret * 64);
        MixPrune(secret / 32);
        MixPrune(secret * 2048);

        return secret;

        void MixPrune(uint value)
        {
            secret = (secret ^ value) % 16777216;
        }
    }
}
