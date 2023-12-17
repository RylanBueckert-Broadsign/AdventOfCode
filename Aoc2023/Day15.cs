using System.Text;
using AoC2023.Utils;

namespace AoC2023;

public static class Day15
{
    public static void Run()
    {
        var input = InputHelper.ReadWholeFile(@"Day15\input.txt");

        var initSequence = input.Split(',');

        var boxes = Enumerable.Range(0, 256).Select(_ => new List<Lens>()).ToArray();

        foreach (var step in initSequence)
        {
            if (step.EndsWith('-'))
            {
                // Remove Operation
                var label = step[..^1];
                var boxIdx = Hash(label);

                boxes[boxIdx].RemoveAll(l => l.Label == label);
            }
            else
            {
                //Add operation
                var eqIdx = step.IndexOf('=');
                var label = step[..eqIdx];
                var focalLength = int.Parse(step[(eqIdx + 1)..]);
                var boxIdx = Hash(label);

                var newLens = new Lens(label, focalLength);

                var existingLensIdx = boxes[boxIdx].FindIndex(l => l.Label == label);

                if (existingLensIdx >= 0)
                {
                    boxes[boxIdx][existingLensIdx] = newLens;
                }
                else
                {
                    boxes[boxIdx].Add(newLens);
                }
            }
        }

        var focusingPower = boxes.Select((lenses, boxIdx) => lenses.Select((lens, lensIdx) => (boxIdx + 1) * (lensIdx + 1) * lens.FocalLength).Sum()).Sum();

        // var hashes = initSequence.Select(Hash);

        Console.WriteLine(focusingPower);
    }

    private record Lens(string Label, int FocalLength);

    private static int Hash(string input) =>
        Encoding.ASCII
            .GetBytes(input)
            .Select(i => (int)i)
            .Aggregate(0, (currentValue, next) => (currentValue + next) * 17 % 256);
}
