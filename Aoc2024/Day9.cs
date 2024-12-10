using Aoc2024.Common;

namespace Aoc2024;

public class Day9 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadWholeFile(inputPath);

        var files = new (int size, int pos)[(input.Length + 1) / 2];
        var gaps = new List<(int size, int pos)>();

        var pos = 0;
        for (var i = 0; i < input.Length; i++)
        {
            var value = int.Parse(input[i].ToString());

            if (i % 2 == 0)
            {
                files[i / 2] = (value, pos);
            }
            else
            {
                gaps.Add((value, pos));
            }

            pos += value;
        }

        for (var file = files.Length - 1; file >= 0; file--)
        {
            // Find first gap that fits file to the left of current position
            var gapIdx = gaps.FindIndex(g => g.size >= files[file].size && g.pos < files[file].pos);

            // No gap found, skip
            if (gapIdx == -1)
                continue;

            // Move file
            files[file] = (files[file].size, gaps[gapIdx].pos);

            // Update gap
            gaps[gapIdx] = (gaps[gapIdx].size - files[file].size, gaps[gapIdx].pos + files[file].size);
        }

        long checkSum = 0;
        for (var file = 0; file < files.Length; file++)
        {
            for (var i = 0; i < files[file].size; i++)
            {
                checkSum += (i + files[file].pos) * file;
            }
        }

        Console.WriteLine(checkSum);
    }

    public void RunPart1(string inputPath)
    {
        var input = InputHelper.ReadWholeFile(inputPath);

        var fileSizes = new int[(input.Length + 1) / 2];
        var gaps = new int[input.Length / 2];

        for (var i = 0; i < input.Length; i++)
        {
            var value = int.Parse(input[i].ToString());

            if (i % 2 == 0)
            {
                fileSizes[i / 2] = value;
            }
            else
            {
                gaps[i / 2] = value;
            }
        }

        long checkSum = 0;

        var pos = 0;
        var backward = fileSizes.Length;
        var backwardBlocksRemaining = 0;
        for (var forward = 0; forward < backward; forward++)
        {
            // Keep file in place
            for (var i = 0; i < fileSizes[forward]; i++)
            {
                checkSum += forward * pos;
                pos++;
            }

            // Fill gap with back files
            for (var i = 0; i < gaps[forward]; i++)
            {
                if (backwardBlocksRemaining == 0)
                {
                    backward--;
                    backwardBlocksRemaining = fileSizes[backward];
                }

                checkSum += backward * pos;
                pos++;
                backwardBlocksRemaining--;
            }
        }

        // Compacted, use final blocks
        for (var i = 0; i < backwardBlocksRemaining; i++)
        {
            checkSum += backward * pos;
            pos++;
        }

        Console.WriteLine(checkSum);
    }
}
