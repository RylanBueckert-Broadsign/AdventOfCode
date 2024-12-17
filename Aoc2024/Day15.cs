using Aoc2024.Common;

namespace Aoc2024;

public class Day15 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath).ToList();

        var grid = input.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.SelectMany(c =>
        {
            return c switch
            {
                'O' => new[] { '[', ']' },
                '@' => new[] { '@', '.' },
                _ => new[] { c, c }
            };
        }).ToArray()).ToArray();

        var instructions = input.Skip(grid.Length + 1).SelectMany(i => i);

        var robot = grid.SelectMany((row, x) => row.Select((c, y) => (c, x, y)).Where(t => t.c == '@')).Select(i => new Vec2D<int>(i.x, i.y)).Single();
        grid[robot.X][robot.Y] = '.';

        var directionMap = new Dictionary<char, Direction>
        {
            ['^'] = Direction.Up,
            ['v'] = Direction.Down,
            ['<'] = Direction.Left,
            ['>'] = Direction.Right,
        };

        foreach (var ins in instructions)
        {
            // for (var x = 0; x < grid.Length; x++)
            // {
            //     for (var y = 0; y < grid[x].Length; y++)
            //     {
            //         if (robot.X == x && robot.Y == y)
            //             Console.Write('@');
            //         else
            //             Console.Write(grid[x][y]);
            //     }
            //
            //     Console.WriteLine();
            // }
            //
            // Console.Read();

            var dir = directionMap[ins];
            var next = robot.Move(dir);

            if (grid[next.X][next.Y] == '#')
                continue;

            if (grid[next.X][next.Y] is '[' or ']')
            {
                if (!CanPush(next, dir))
                    continue;

                ForcePush(next, dir);

                // var push = next.Move(directionMap[ins]);
                //
                // while (grid[push.X][push.Y] is '[' or ']')
                // {
                //     push = push.Move(directionMap[ins]);
                // }
                //
                // if (grid[push.X][push.Y] == '#')
                //     continue;

                // grid[next.X][next.Y] = '.';
                // grid[push.X][push.Y] = 'O';
            }

            robot = next;
        }

        var boxes = grid.SelectMany((row, x) => row.Select((c, y) => (c, x, y)).Where(t => t.c == '[')).Select(i => new Vec2D<int>(i.x, i.y));

        var gps = boxes.Select(b => b.X * 100 + b.Y);

        Console.WriteLine(gps.Sum());

        return;

        bool CanPush(Vec2D<int> box, Direction dir)
        {
            if (dir is Direction.Up or Direction.Down)
            {
                Vec2D<int> left, right;

                if (grid[box.X][box.Y] == '[')
                {
                    left = box;
                    right = box.Move(Direction.Right);
                }
                else if (grid[box.X][box.Y] == ']')
                {
                    left = box.Move(Direction.Left);
                    right = box;
                }
                else
                {
                    throw new Exception("Not a box");
                }

                var nextLeft = left.Move(dir);
                var nextRight = right.Move(dir);

                if (grid[nextLeft.X][nextLeft.Y] == '#' || grid[nextRight.X][nextRight.Y] == '#')
                    return false;

                var canPushLeft = grid[nextLeft.X][nextLeft.Y] == '.' || CanPush(nextLeft, dir);

                return canPushLeft && (grid[nextRight.X][nextRight.Y] == '.' || CanPush(nextRight, dir));
            }
            else
            {
                var next = box.Move(dir);

                while (grid[next.X][next.Y] is '[' or ']')
                {
                    next = next.Move(dir);
                }

                return grid[next.X][next.Y] == '.';
            }
        }

        void ForcePush(Vec2D<int> box, Direction dir)
        {
            if (dir is Direction.Up or Direction.Down)
            {
                Vec2D<int> left, right;

                if (grid[box.X][box.Y] == '[')
                {
                    left = box;
                    right = box.Move(Direction.Right);
                }
                else if (grid[box.X][box.Y] == ']')
                {
                    left = box.Move(Direction.Left);
                    right = box;
                }
                else
                {
                    throw new Exception("Not a box");
                }

                var nextLeft = left.Move(dir);
                var nextRight = right.Move(dir);

                if (grid[nextLeft.X][nextLeft.Y] != '.')
                {
                    ForcePush(nextLeft, dir);
                }

                grid[nextLeft.X][nextLeft.Y] = '[';
                grid[left.X][left.Y] = '.';

                if (grid[nextRight.X][nextRight.Y] != '.')
                {
                    ForcePush(nextRight, dir);
                }

                grid[nextRight.X][nextRight.Y] = ']';
                grid[right.X][right.Y] = '.';
            }
            else
            {
                var emptySpace = box.Move(dir);

                while (grid[emptySpace.X][emptySpace.Y] is '[' or ']')
                {
                    emptySpace = emptySpace.Move(dir);
                }

                while (true)
                {
                    var reverseDir = dir.TurnLeft(2);
                    var next = emptySpace.Move(reverseDir);
                    grid[emptySpace.X][emptySpace.Y] = grid[next.X][next.Y];

                    emptySpace = next;

                    if (emptySpace == box)
                        break;
                }

                grid[emptySpace.X][emptySpace.Y] = '.';
            }
        }
    }
}