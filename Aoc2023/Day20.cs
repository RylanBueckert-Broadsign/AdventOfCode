using AoC2023.Utils;

namespace AoC2023;

public static class Day20
{
    public static void Run()
    {
        var input = InputHelper.ReadLines(@"Day20\input.txt");

        var configs = input.Select(i =>
        {
            var arrowIdx = i.IndexOf("->", StringComparison.Ordinal);

            var outputs = i[(arrowIdx + 2)..].Split(',').Select(j => j.Trim()).ToList();

            var nodeDef = i[..arrowIdx].Trim();

            if (nodeDef == "broadcaster")
            {
                return new NodeConfig(NodeType.Broadcaster, nodeDef, outputs);
            }

            if (nodeDef.StartsWith('%'))
            {
                return new NodeConfig(NodeType.FlipFlop, nodeDef[1..], outputs);
            }

            if (nodeDef.StartsWith('&'))
            {
                return new NodeConfig(NodeType.Conjunction, nodeDef[1..], outputs);
            }

            return new NodeConfig(NodeType.None, nodeDef, outputs);
        }).ToList();

        var network = new Dictionary<string, INode>();

        foreach (var config in configs)
        {
            switch (config.Type)
            {
                case NodeType.Broadcaster:
                    network.Add(config.Name, new Broadcaster(config.Outputs));
                    break;
                case NodeType.FlipFlop:
                    network.Add(config.Name, new FlipFlop(config.Outputs));
                    break;
                case NodeType.Conjunction:
                    var inputs = configs.Where(c => c.Outputs.Contains(config.Name)).Select(c => c.Name).ToList();
                    network.Add(config.Name, new Conjunction(config.Outputs, inputs));
                    break;
                default:
                    network.Add(config.Name, new Dummy());
                    break;
            }
        }

        foreach (var outputOnly in configs.SelectMany(config => config.Outputs).Distinct().Where(o => !network.ContainsKey(o)))
        {
            network.Add(outputOnly, new Dummy());
        }

        // long lowPulses = 0;
        // long highPulses = 0;

        // foreach (var _ in Enumerable.Range(0, 1000))
        // {
        //     PressButton();
        // }
        //
        // Console.WriteLine(lowPulses * highPulses);

        long? ddDone = null;
        long? fhDone = null;
        long? xpDone = null;
        long? fcDone = null;

        for (var i = 1;; i++)
        {
            PressButton(i);

            if (ddDone != null && fhDone != null && xpDone != null && fcDone != null)
            {
                break;
            }
        }

        var lcm = Lcm(Lcm(ddDone.Value, fhDone.Value), Lcm(xpDone.Value, fcDone.Value));

        Console.WriteLine(lcm);

        // Console.WriteLine(lowPulses * highPulses);

        return;

        void PressButton(int pressNum)
        {
            var pulseQueue = new Queue<(string target, string from, bool isHigh)>();

            pulseQueue.Enqueue(("broadcaster", "button", false));

            while (pulseQueue.TryDequeue(out var pulse))
            {
                // lowPulses += pulse.isHigh ? 0 : 1;
                // highPulses += pulse.isHigh ? 1 : 0;

                var pulseResult = network[pulse.target].Pulse(pulse.from, pulse.isHigh);
                foreach (var newPulse in pulseResult.outputs)
                {
                    if (ddDone == null && newPulse == "dd" && !pulseResult.isHigh)
                    {
                        ddDone = pressNum;
                    }

                    if (fhDone == null && newPulse == "fh" && !pulseResult.isHigh)
                    {
                        fhDone = pressNum;
                    }

                    if (xpDone == null && newPulse == "xp" && !pulseResult.isHigh)
                    {
                        xpDone = pressNum;
                    }

                    if (fcDone == null && newPulse == "fc" && !pulseResult.isHigh)
                    {
                        fcDone = pressNum;
                    }

                    pulseQueue.Enqueue((newPulse, pulse.target, pulseResult.isHigh));
                    // Console.WriteLine($"{newPulse} received {(pulseResult.isHigh ? "HIGH" : "LOW")} pulse from {pulse.target}");
                }
            }
        }
    }

    private static long Gcf(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    private static long Lcm(long a, long b)
    {
        return (a / Gcf(a, b)) * b;
    }

    private record NodeConfig(NodeType Type, string Name, List<string> Outputs);

    private enum NodeType
    {
        Broadcaster,
        FlipFlop,
        Conjunction,
        None
    }

    private interface INode
    {
        (IReadOnlyList<string> outputs, bool isHigh) Pulse(string from, bool isHigh);
    }

    private abstract class Node : INode
    {
        protected readonly List<string> _outputs;

        protected Node(IEnumerable<string> outputs)
        {
            _outputs = outputs.ToList();
        }

        public abstract (IReadOnlyList<string> outputs, bool isHigh) Pulse(string from, bool isHigh);
    }

    private class Broadcaster : Node
    {
        public Broadcaster(IEnumerable<string> outputs)
            : base(outputs)
        {
        }

        public override (IReadOnlyList<string> outputs, bool isHigh) Pulse(string from, bool isHigh)
        {
            return (_outputs, isHigh);
        }
    }

    private class FlipFlop : Node
    {
        private bool _isOn;

        public FlipFlop(IEnumerable<string> outputs)
            : base(outputs)
        {
        }

        public override (IReadOnlyList<string> outputs, bool isHigh) Pulse(string from, bool isHigh)
        {
            if (isHigh)
                return (new List<string>(), false);

            _isOn = !_isOn;

            return (_outputs, _isOn);
        }
    }

    private class Conjunction : Node
    {
        private readonly Dictionary<string, bool> _inputMemory;

        public Conjunction(IEnumerable<string> outputs, IEnumerable<string> inputs)
            : base(outputs)
        {
            _inputMemory = inputs.ToDictionary(i => i, _ => false);
        }

        public override (IReadOnlyList<string> outputs, bool isHigh) Pulse(string from, bool isHigh)
        {
            _inputMemory[from] = isHigh;

            return (_outputs, !_inputMemory.All(i => i.Value));
        }
    }

    private class Dummy : INode
    {
        public (IReadOnlyList<string> outputs, bool isHigh) Pulse(string from, bool isHigh)
        {
            return (new List<string>(), false);
        }
    }
}
