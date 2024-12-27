using System.Text.RegularExpressions;
using Aoc2024.Common;

namespace Aoc2024;

public partial class Day24 : IAocDay
{
    private struct Logic
    {
        public string First { get; init; }
        public string Second { get; init; }
        public string Operation { get; init; }
    }

    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath).ToList();

        var stateInput = input.TakeWhile(i => !string.IsNullOrWhiteSpace(i)).ToList();

        var logicInput = input.Skip(stateInput.Count + 1).ToList();

        // Part 1
        var memory = stateInput.Select(i =>
        {
            var parts = i.Split(':', StringSplitOptions.TrimEntries);

            return (key: parts[0], value: int.Parse(parts[1]));
        }).ToDictionary(i => i.key, i => i.value > 0);

        var logic = logicInput.Select(i => LogicRegex().Match(i).Groups).ToDictionary(i => i["result"].Value, i => new Logic()
        {
            First = i["first"].Value,
            Second = i["second"].Value,
            Operation = i["op"].Value
        });

        long result = 0;

        for (var digit = 0;; digit++)
        {
            var key = $"z{digit.ToString().PadLeft(2, '0')}";

            var value = GetValue(key);

            if (value == null)
                break;

            result += (long)(value.Value ? 1 : 0) << digit;
        }

        Console.WriteLine(result);

        // Part 2
        Swap("qwf", "cnk");
        Swap("z14", "vhm");
        Swap("z27", "mps");
        Swap("z39", "msq");

        var cNames = new string[45];

        var d00 = FindHalfAdderOutputs("x00", "y00");
        cNames[0] = d00.and!;

        if (d00.xor != "z00")
        {
            throw new Exception("z00");
        }

        for (var digit = 1; digit <= 44; digit++)
        {
            var x = $"x{digit.ToString().PadLeft(2, '0')}";
            var y = $"y{digit.ToString().PadLeft(2, '0')}";
            var z = $"z{digit.ToString().PadLeft(2, '0')}";

            var firstAdder = FindHalfAdderOutputs(x, y);

            var secondAdder = FindHalfAdderOutputs(firstAdder.xor!, cNames[digit - 1]);

            if (secondAdder.xor is null)
            {
                throw new Exception($"{z}: {firstAdder.xor} or {cNames[digit - 1]}");
            }

            if (secondAdder.and is null)
            {
                throw new Exception($"{z} carry AND: {firstAdder.xor} or {cNames[digit - 1]}");
            }

            var carry = FindOutputName(secondAdder.and, firstAdder.and!, "OR");

            if (carry is null)
            {
                throw new Exception($"{z} carry OR: {secondAdder.and} or {firstAdder.and!}");
            }

            cNames[digit] = carry;

            if (z != secondAdder.xor)
            {
                throw new Exception($"{z} swap with {secondAdder.xor}");
            }
        }

        Console.WriteLine("Adder is fixed");
        return;

        void Swap(string key1, string key2)
        {
            (logic[key1], logic[key2]) = (logic[key2], logic[key1]);
        }

        (string? xor, string? and) FindHalfAdderOutputs(string in1, string in2)
        {
            return (FindOutputName(in1, in2, "XOR"), FindOutputName(in1, in2, "AND"));
        }

        string? FindOutputName(string in1, string in2, string op)
        {
            return logic.FirstOrDefault(l =>
                l.Value.Operation == op &&
                ((l.Value.First == in1 && l.Value.Second == in2) ||
                    (l.Value.Second == in1 && l.Value.First == in2))
            ).Key;
        }

        bool? GetValue(string key)
        {
            if (!memory.TryGetValue(key, out var value))
            {
                if (!logic.TryGetValue(key, out var calc))
                    return null;

                var first = GetValue(calc.First);
                var second = GetValue(calc.Second);

                if (first == null || second == null)
                    return null;

                value = calc.Operation switch
                {
                    "AND" => first.Value & second.Value,
                    "OR" => first.Value | second.Value,
                    "XOR" => first.Value ^ second.Value,
                    _ => throw new Exception("Unknown operation")
                };

                memory[key] = value;
            }

            return value;
        }
    }

    [GeneratedRegex(@"(?<first>\w{3}) (?<op>[A-Z]+) (?<second>\w{3}) -> (?<result>\w{3})")]
    private static partial Regex LogicRegex();
}
