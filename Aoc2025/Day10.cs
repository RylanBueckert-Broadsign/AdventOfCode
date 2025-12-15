using Aoc2025.Common;

namespace Aoc2025;

public class Day10 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath).ToList();

        Part1(input);
        
        // Too slow to finish in reasonable time
        //Part2(input);
    }

    private static void Part1(IEnumerable<string> input)
    {
        var cases = input.Select(line =>
        {
            var lightEnd = line.IndexOf(']');
            var joltageStart = line.IndexOf('{');

            var light = line[1..lightEnd].Select((c, idx) => (c == '#' ? 1 << idx : 0)).Sum();

            var buttons = line[(lightEnd + 2)..(joltageStart - 1)]
                .Split(' ')
                .Select(b => b[1..^1])
                .Select(b => b
                    .Split(',')
                    .Select(int.Parse)
                    .Aggregate(0, (acc, n) => acc + (1 << n)))
                .ToList();

            return (light, buttons);
        }).ToList();

        var buttonPresses = cases
            .Select(c => FewestButtonPressesLights(0, c.light, c.buttons));

        Console.WriteLine(buttonPresses.Sum());
    }

    
    private static void Part2(IEnumerable<string> input)
    {
        var cases = input.Select(line =>
        {
            var lightEnd = line.IndexOf(']');
            var joltageStart = line.IndexOf('{');

            var buttons = line[(lightEnd + 2)..(joltageStart - 1)]
                .Split(' ')
                .Select(b => b[1..^1])
                .Select(b => b
                    .Split(',')
                    .Select(int.Parse)
                    .ToList())
                .ToList();

            var joltageValues = line[(joltageStart + 1)..^1]
                .Split(',')
                .Select(int.Parse)
                .ToList();

            return (joltageValues, buttons);
        }).ToList();

        var buttonPresses = cases
            .Select(c => FewestButtonPressesJoltage(c.joltageValues, c.buttons));

        Console.WriteLine(buttonPresses.Sum());
    }

    private static int FewestButtonPressesLights(int start, int target, List<int> buttons)
    {
        var queue = new Queue<(int state, int distance)>();
        queue.Enqueue((start, 0));

        var visited = new HashSet<int>();

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (!visited.Add(current.state))
            {
                continue;
            }

            if (current.state == target)
            {
                return current.distance;
            }

            foreach (var button in buttons)
            {
                var nextState = current.state ^ button;
                queue.Enqueue((nextState, current.distance + 1));
            }
        }

        throw new Exception(); // Target not reachable
    }

    private static int? FewestButtonPressesJoltage(List<int> target, List<List<int>> buttons)
    {
        var sortedButtons = buttons
            .OrderByDescending(b => b.Count)
            .ToList();

        int? best = null;

        SolveJoltage(target.ToArray(), 0, new int[buttons.Count], 0);

        return best;

        void SolveJoltage(int[] currentTarget, int buttonIndex, int[] currentPresses, int currentCount)
        {
            if (currentCount >= best)
                return;

            if (currentTarget.All(p => p == 0))
            {
                best = currentCount;
                return;
            }

            if (buttonIndex >= sortedButtons.Count)
                return;

            var currentButton = sortedButtons[buttonIndex];

            var maxK = int.MaxValue;

            foreach (var idx in currentButton)
            {
                if (currentTarget[idx] == 0)
                {
                    maxK = 0;
                    break;
                }
                
                if (currentTarget[idx] < maxK)
                {
                    maxK = currentTarget[idx];
                }
            }

            for (var k = maxK; k >= 0; k--)
            {
                if (currentCount + k >= best)
                    continue;
                
                currentPresses[buttonIndex] = k;

                foreach (var idx in currentButton)
                {
                    currentTarget[idx] -= k;
                }
                
                SolveJoltage(currentTarget, buttonIndex + 1, currentPresses, currentCount + k);
                
                foreach (var idx in currentButton)
                {
                    currentTarget[idx] += k;
                }
            }
            
            currentPresses[buttonIndex] = 0;
        }
    }
}
