using Aoc2024.Common;

namespace Aoc2024;

public class Day17 : IAocDay
{
    public void Run(string inputPath)
    {
        var input = InputHelper.ReadLines(inputPath).Where(i => !string.IsNullOrWhiteSpace(i)).ToList();

        // var registerA = int.Parse(ReadValue(input[0]));
        var registerB = long.Parse(ReadValue(input[1]));
        var registerC = long.Parse(ReadValue(input[2]));

        var program = ReadValue(input[3]).Split(',').Select(int.Parse).ToList();

        long registerA = 0;

        for (var i = program.Count - 1; i >= 0; i--)
        {
            registerA <<= 3;

            for (var digit = 0; digit < 8; digit++)
            {
                var output = RunProgram(program, registerA + digit, registerB, registerC);

                if (output[0] == program[i])
                {
                    registerA += digit;
                    break;
                }

                while (digit >= 7)
                {
                    // No match, need to backtrack
                    i++;
                    registerA /= 8;
                    digit = (int)(registerA % 8);
                    registerA -= digit;
                }
            }
        }

        Console.WriteLine(registerA);
        Console.WriteLine(string.Join(",", program));
        Console.WriteLine(string.Join(",", RunProgram(program, registerA, registerB, registerC)));
    }

    private static List<int> RunProgram(List<int> program, long registerA, long registerB, long registerC)
    {
        var instructionPointer = 0;

        var output = new List<int>();

        Action<int>[] instructions =
        [
            adv,
            bxl,
            bst,
            jnz,
            bxc,
            @out,
            bdv,
            cdv
        ];

        while (instructionPointer < program.Count - 1)
        {
            instructions[program[instructionPointer]](program[instructionPointer + 1]);

            instructionPointer += 2;
        }

        return output;

        long Combo(int operand)
        {
            return operand switch
            {
                0 or 1 or 2 or 3 => operand,
                4 => registerA,
                5 => registerB,
                6 => registerC,
                _ => throw new ArgumentOutOfRangeException(nameof(operand), $"Invalid combo operand: {operand}")
            };
        }

        // ReSharper disable InconsistentNaming
        void adv(int operand)
        {
            registerA /= 1 << (int)Combo(operand);
        }

        void bxl(int operand)
        {
            registerB ^= operand;
        }

        void bst(int operand)
        {
            registerB = Combo(operand) % 8;
        }

        void jnz(int operand)
        {
            if (registerA != 0)
            {
                instructionPointer = operand - 2;
            }
        }

        void bxc(int operand)
        {
            registerB ^= registerC;
        }

        void @out(int operand)
        {
            output.Add((int)(Combo(operand) % 8));
        }

        void bdv(int operand)
        {
            registerB = registerA / (1 << (int)Combo(operand));
        }

        void cdv(int operand)
        {
            registerC = registerA / (1 << (int)Combo(operand));
        }
        // ReSharper restore InconsistentNaming
    }

    private static string ReadValue(string line)
    {
        return line[(line.IndexOf(':') + 2)..].Trim();
    }
}
