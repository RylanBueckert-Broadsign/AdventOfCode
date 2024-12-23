﻿namespace Aoc2024.Common;

public static class InputHelper
{
    public static string ReadWholeFile(string path)
    {
        var sr = new StreamReader(@$".\input\{path}");
        return sr.ReadToEnd().Trim();
    }

    public static IEnumerable<string> ReadLines(string path) =>
        ReadWholeFile(path).Split('\n', StringSplitOptions.TrimEntries);

    public static char[][] ReadGrid(string path) =>
        ReadGrid(path, c => c);

    public static T[][] ReadGrid<T>(string path, Func<char, T> transformFunc) =>
        ReadLines(path).Select(i => i.Select(transformFunc).ToArray()).ToArray();
}
