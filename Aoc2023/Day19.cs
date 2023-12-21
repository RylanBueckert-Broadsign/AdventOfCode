using System.Text.RegularExpressions;
using AoC2023.Utils;

namespace AoC2023;

public static class Day19
{
    public static void Run()
    {
        var input = InputHelper.ReadLines(@"Day19\input.txt").ToList();

        var workflowsStr = input.TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToList();
        // var partsStr = input.Skip(workflowsStr.Count + 1);

        var workflows = new Dictionary<string, IWorkflow>();
        var accepted = new DestinationWorkflow();
        var rejected = new DestinationWorkflow();

        foreach (var parseResult in workflowsStr.Select(ParseWorkflow))
        {
            workflows.Add(parseResult.name, parseResult.workflow);
        }

        workflows.Add("A", accepted);
        workflows.Add("R", rejected);

        // var parts = partsStr.Select(ParsePart);

        // foreach (var part in parts)
        // {
        //     var targetWorkflow = "in";
        //
        //     while (targetWorkflow is not null)
        //     {
        //         targetWorkflow = workflows[targetWorkflow].Execute(part);
        //     }
        // }

        // var ratings = accepted.Parts.Select(p => p.X + p.M + p.A + p.S);
        //
        // Console.WriteLine(ratings.Sum());

        var allParts = new Part(new Range(1, 4000), new Range(1, 4000), new Range(1, 4000), new Range(1, 4000));

        var toProcess = new Queue<(string targetWorkflow, Part part)>();
        
        toProcess.Enqueue(("in", allParts));

        while (toProcess.TryDequeue(out var next))
        {
            foreach (var newWorkflow in workflows[next.targetWorkflow].Execute(next.part))
            {
                toProcess.Enqueue(newWorkflow);
            }
        }

        var acceptedCount = accepted.Parts.Select(p => p.TotalCount()).Sum();
        
        Console.WriteLine(acceptedCount);
    }

    // private static Part ParsePart(string partStr)
    // {
    //     var x = int.Parse(Regex.Match(partStr, @"(?<=x=)\d+").Value);
    //     var m = int.Parse(Regex.Match(partStr, @"(?<=m=)\d+").Value);
    //     var a = int.Parse(Regex.Match(partStr, @"(?<=a=)\d+").Value);
    //     var s = int.Parse(Regex.Match(partStr, @"(?<=s=)\d+").Value);
    //
    //     return new Part(x, m, a, s);
    // }

    private static (string name, RuleWorkflow workflow) ParseWorkflow(string input)
    {
        var name = Regex.Match(input, @"\w+(?={.+})").Value;
        var rulesStr = Regex.Match(input, @"(?<=\w{).+(?=})").Value;

        var splitRules = rulesStr.Split(',');

        var rules = splitRules.Take(splitRules.Length - 1).Select(r =>
        {
            var dest = Regex.Match(r, @"(?<=:)\w+").Value;

            var param = Regex.Match(r, @"\w(?=[<>]\d+)").Value switch
            {
                "x" => PartParameter.X,
                "m" => PartParameter.M,
                "a" => PartParameter.A,
                "s" => PartParameter.S,
                _ => throw new Exception("Invalid parameter")
            };

            var operation = Regex.Match(r, @"(?<=\w)[<>](?=\d+)").Value switch
            {
                "<" => Operation.LessThan,
                ">" => Operation.GreaterThan,
                _ => throw new Exception("Invalid operation")
            };

            var value = int.Parse(Regex.Match(r, @"(?<=\w[<>])\d+").Value);

            return new Rule(param, operation, value, dest);
        });

        return (name, new RuleWorkflow(rules, splitRules.Last()));
    }

    private class Part : Dictionary<PartParameter, Range>
    {
        public Part(Range x, Range m, Range a, Range s)
        {
            Add(PartParameter.X, x);
            Add(PartParameter.M, m);
            Add(PartParameter.A, a);
            Add(PartParameter.S, s);
        }

        public long TotalCount()
        {
            return this.Select(i => (long)i.Value.Count).Aggregate((curr, next) => curr * next);
        }

        public Part Duplicate()
        {
            return new Part(this[PartParameter.X], this[PartParameter.M], this[PartParameter.A], this[PartParameter.S]);
        }
    }

    private record Range(int First, int Last)
    {
        public int Count => Last - First + 1;
    }

    private enum Operation
    {
        LessThan,
        GreaterThan
    }

    private enum PartParameter
    {
        X,
        M,
        A,
        S
    }

    private record Rule(PartParameter Parameter, Operation Operation, int Value, string Destination);

    private interface IWorkflow
    {
        List<(string targetWorkflow, Part part)> Execute(Part part);
    }

    private class RuleWorkflow : IWorkflow
    {
        private readonly List<Rule> _rules;
        private readonly string _defaultDestination;

        public RuleWorkflow(IEnumerable<Rule> rules, string defaultDestination)
        {
            _rules = rules.ToList();
            _defaultDestination = defaultDestination;
        }

        public List<(string targetWorkflow, Part part)> Execute(Part part)
        {
            var remaining = part.Duplicate();
            var result = new List<(string targetWorkflow, Part part)>();

            foreach (var rule in _rules)
            {
                if (remaining.TotalCount() <= 0) return result;

                var partParamRange = part[rule.Parameter];

                if (partParamRange.First > rule.Value
                    || (partParamRange.First == rule.Value && rule.Operation == Operation.LessThan)
                    || partParamRange.Last < rule.Value
                    || (partParamRange.Last == rule.Value && rule.Operation == Operation.GreaterThan))
                {
                    continue;
                }

                var rulePart = remaining.Duplicate();
                if (rule.Operation == Operation.LessThan)
                {
                    rulePart[rule.Parameter] = new Range(remaining[rule.Parameter].First, rule.Value - 1);
                    remaining[rule.Parameter] = new Range(rule.Value, remaining[rule.Parameter].Last);
                }
                else
                {
                    rulePart[rule.Parameter] = new Range(rule.Value + 1, remaining[rule.Parameter].Last);
                    remaining[rule.Parameter] = new Range(remaining[rule.Parameter].First, rule.Value);
                }

                result.Add((rule.Destination, rulePart));
            }

            if (remaining.TotalCount() > 0)
            {
                result.Add((_defaultDestination, remaining));
            }

            return result;
        }
    }

    private class DestinationWorkflow : IWorkflow
    {
        private readonly List<Part> _parts = new();

        public IReadOnlyList<Part> Parts => _parts;

        public List<(string targetWorkflow, Part part)> Execute(Part part)
        {
            _parts.Add(part);

            return new();
        }
    }
}
