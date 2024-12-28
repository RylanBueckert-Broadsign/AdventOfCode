using Aoc2024.Common;

namespace Aoc2024;

public class Day20 : IAocDay
{
    public void Run(string inputPath)
    {
        var grid = InputHelper.ReadGrid(inputPath);

        var end = grid
            .SelectMany((row, x) => row.Select((cell, y) => (cell, x, y)))
            .Where(c => c.cell == 'E')
            .Select(c => new Vec2D<int>(c.x, c.y))
            .First();

        var basePath = new Dictionary<Vec2D<int>, int>();

        var curr = end;
        var distance = 0;

        while (true)
        {
            basePath[curr] = distance;

            if (grid[curr.X][curr.Y] == 'S')
                break;

            curr = Enum.GetValues<Direction>()
                .Select(dir => curr.Move(dir))
                .Single(newPos => grid[newPos.X][newPos.Y] != '#' && !basePath.ContainsKey(newPos));
            distance++;
        }

        var cheats = basePath.SelectMany(kv =>
        {
            var pos = kv.Key;
            var dist = kv.Value;

            var endPositions = Enumerable.Range(-20, 41)
                .SelectMany(cheatX =>
                {
                    var leftOverCheat = 20 - Math.Abs(cheatX);
                    return Enumerable.Range(-leftOverCheat, leftOverCheat * 2 + 1)
                        .Select(cheatY => new
                        {
                            pos = new Vec2D<int>(pos.X + cheatX, pos.Y + cheatY),
                            cheatTime = Math.Abs(cheatX) + Math.Abs(cheatY)
                        });
                });

            return endPositions
                .Where(e => basePath.ContainsKey(e.pos))
                .Select(e => new
                {
                    start = pos,
                    end = e,
                    timeSaved = dist - basePath[e.pos] - e.cheatTime
                })
                .Where(c => c.timeSaved > 0);
        });

        Console.WriteLine(cheats.Count(c => c.timeSaved >= 100));
    }
}
