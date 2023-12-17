using AoC2023.Utils;

namespace AoC2023;

public static class Day5
{
    private static readonly MapLookup SeedSoilMap = new();
    private static readonly MapLookup SoilFertilizerMap = new();
    private static readonly MapLookup FertilizerWaterMap = new();
    private static readonly MapLookup WaterLightMap = new();
    private static readonly MapLookup LightTempMap = new();
    private static readonly MapLookup TempHumidMap = new();
    private static readonly MapLookup HumidLocationMap = new();

    public static void Run()
    {
        var input = InputHelper.ReadWholeFile(@"Day5\input.txt");

        const string s0 = "seeds:";
        const string s1 = "seed-to-soil map:";
        const string s2 = "soil-to-fertilizer map:";
        const string s3 = "fertilizer-to-water map:";
        const string s4 = "water-to-light map:";
        const string s5 = "light-to-temperature map:";
        const string s6 = "temperature-to-humidity map:";
        const string s7 = "humidity-to-location map:";
        var seedSoilMapIdx = input.IndexOf(s1, StringComparison.Ordinal);
        var soilFertilizerMapIdx = input.IndexOf(s2, StringComparison.Ordinal);
        var fertilizerWaterMapIdx = input.IndexOf(s3, StringComparison.Ordinal);
        var waterLightMapIdx = input.IndexOf(s4, StringComparison.Ordinal);
        var lightTempMapIdx = input.IndexOf(s5, StringComparison.Ordinal);
        var tempHumidMapIdx = input.IndexOf(s6, StringComparison.Ordinal);
        var humidLocationMapIdx = input.IndexOf(s7, StringComparison.Ordinal);

        var seeds = input[s0.Length..seedSoilMapIdx].Trim().Split(' ').Select(long.Parse).ToList();

        var seedRanges = new List<Range>();

        for (var i = 0; i < seeds.Count; i += 2)
        {
            seedRanges.Add(new Range(seeds[i], seeds[i + 1]));
        }

        PopulateMap(SeedSoilMap, input[(seedSoilMapIdx + s1.Length)..soilFertilizerMapIdx]);
        PopulateMap(SoilFertilizerMap, input[(soilFertilizerMapIdx + s2.Length)..fertilizerWaterMapIdx]);
        PopulateMap(FertilizerWaterMap, input[(fertilizerWaterMapIdx + s3.Length)..waterLightMapIdx]);
        PopulateMap(WaterLightMap, input[(waterLightMapIdx + s4.Length)..lightTempMapIdx]);
        PopulateMap(LightTempMap, input[(lightTempMapIdx + s5.Length)..tempHumidMapIdx]);
        PopulateMap(TempHumidMap, input[(tempHumidMapIdx + s6.Length)..humidLocationMapIdx]);
        PopulateMap(HumidLocationMap, input[(humidLocationMapIdx + s7.Length)..]);

        // var locations = seeds
        //     .Select(SeedSoilMap.Lookup)
        //     .Select(SoilFertilizerMap.Lookup)
        //     .Select(FertilizerWaterMap.Lookup)
        //     .Select(WaterLightMap.Lookup)
        //     .Select(LightTempMap.Lookup)
        //     .Select(TempHumidMap.Lookup)
        //     .Select(HumidLocationMap.Lookup);

        var locationRanges = seedRanges
            .SelectMany(SeedSoilMap.MapRange)
            .SelectMany(SoilFertilizerMap.MapRange)
            .SelectMany(FertilizerWaterMap.MapRange)
            .SelectMany(WaterLightMap.MapRange)
            .SelectMany(LightTempMap.MapRange)
            .SelectMany(TempHumidMap.MapRange)
            .SelectMany(HumidLocationMap.MapRange);

        var result = locationRanges.Where(i => i.Length > 0).Select(i => i.Start).Min();

        Console.WriteLine(result);
    }

    private static void PopulateMap(MapLookup map, string text)
    {
        var clean = text.Trim();
        var lines = clean.Split('\n');

        foreach (var line in lines)
        {
            var nums = line.Split(' ').Select(long.Parse).ToList();
            var destStart = nums[0];
            var sourceStart = nums[1];
            var length = nums[2];

            map.AddMap(sourceStart, destStart, length);
        }
    }

    private record Range(long Start, long Length);

    private class MapLookup
    {
        private readonly SortedList<long, Range> _maps = new();

        public void AddMap(long sourceStart, long destStart, long length)
        {
            _maps.Add(sourceStart, new Range(destStart, length));
        }

        public long Lookup(long source)
        {
            foreach (var map in _maps)
            {
                if (source < map.Key)
                    return source;

                if (source <= map.Key + map.Value.Length)
                {
                    var distance = source - map.Key;
                    return map.Value.Start + distance;
                }
            }

            return source;
        }

        public IEnumerable<Range> MapRange(Range range)
        {
            var remaining = range;

            var newRanges = new List<Range>();

            foreach (var map in _maps)
            {
                //Before
                if (remaining.Start < map.Key)
                {
                    var end = Math.Min(remaining.Start + remaining.Length, map.Key);
                    var before = new Range(remaining.Start, end - remaining.Start);
                    newRanges.Add(before);

                    remaining = new Range(end, remaining.Length - before.Length);
                }

                if (remaining.Length <= 0)
                    break;

                //Inside
                if (remaining.Start <= map.Key + map.Value.Length)
                {
                    var end = Math.Min(remaining.Start + remaining.Length, map.Key + map.Value.Length);
                    var offset = map.Value.Start - map.Key;
                    var inside = new Range(remaining.Start + offset, end - remaining.Start);
                    newRanges.Add(inside);

                    remaining = new Range(end, remaining.Length - inside.Length);
                }

                if (remaining.Length <= 0)
                    break;
            }

            if (remaining.Length > 0)
                newRanges.Add(remaining);

            return newRanges;
        }
    }
}
