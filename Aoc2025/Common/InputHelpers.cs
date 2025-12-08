namespace Aoc2025.Common;

public static class InputHelper
{
    public static string ReadWholeFile(string path, bool trim = true)
    {
        using var sr = new StreamReader(@$".\input\{path}");

        var content = sr.ReadToEnd().Replace("\r", "");
        return trim ? content.Trim() : content;
    }

    public static IEnumerable<string> ReadLines(string path, bool trim = true) =>
        ReadWholeFile(path, trim).Split('\n', trim ? StringSplitOptions.TrimEntries : StringSplitOptions.None).Where(l => !string.IsNullOrWhiteSpace(l));

    public static char[][] ReadGrid(string path) =>
        ReadGrid(path, c => c);

    public static T[][] ReadGrid<T>(string path, Func<char, T> transformFunc) =>
        ReadLines(path).Select(i => i.Select(transformFunc).ToArray()).ToArray();
}
