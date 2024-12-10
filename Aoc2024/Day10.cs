using Aoc2024.Common;
using AoC2024.Common;

namespace Aoc2024;

public class Day10 : IAocDay
{
    public void Run(string inputPath)
    {
        var map = InputHelper.ReadGrid(inputPath, c => int.Parse(c.ToString()));

        var startPoints = new List<Vec2D<int>>();

        for (var x = 0; x < map.Length; x++)
        {
            for (var y = 0; y < map[x].Length; y++)
            {
                if (map[x][y] == 0)
                {
                    startPoints.Add((x, y));
                }
            }
        }

        var trailheadCache = new Dictionary<Vec2D<int>, HashSet<Vec2D<int>>>();
        var scores = startPoints.Select(start => GetEndPoints(start).Count);
        Console.WriteLine(scores.Sum());

        var ratingCache = new Dictionary<Vec2D<int>, int>();
        var ratings = startPoints.Select(GetRating);
        Console.WriteLine(ratings.Sum());

        return;

        bool InBounds(Vec2D<int> pos)
        {
            return pos.X >= 0 && pos.X < map.Length && pos.Y >= 0 && pos.Y < map[pos.X].Length;
        }

        HashSet<Vec2D<int>> GetEndPoints(Vec2D<int> pos)
        {
            if (!trailheadCache.TryGetValue(pos, out var endPoints))
            {
                if (map[pos.X][pos.Y] == 9)
                {
                    endPoints = [pos];
                }
                else
                {
                    var canMoveTo = Enum.GetValues<Direction>().Select(dir => pos.Move(dir)).Where(next => InBounds(next) && map[next.X][next.Y] == map[pos.X][pos.Y] + 1);

                    endPoints = canMoveTo.SelectMany(GetEndPoints).ToHashSet();
                }

                trailheadCache[pos] = endPoints;
            }

            return endPoints;
        }

        int GetRating(Vec2D<int> pos)
        {
            if (!ratingCache.TryGetValue(pos, out var rating))
            {
                if (map[pos.X][pos.Y] == 9)
                {
                    rating = 1;
                }
                else
                {
                    var canMoveTo = Enum.GetValues<Direction>().Select(dir => pos.Move(dir)).Where(next => InBounds(next) && map[next.X][next.Y] == map[pos.X][pos.Y] + 1);

                    rating = canMoveTo.Select(GetRating).Sum();
                }

                ratingCache[pos] = rating;
            }

            return rating;
        }
    }
}
