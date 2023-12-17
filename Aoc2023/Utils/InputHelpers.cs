namespace AoC2023.Utils;

public static class InputHelper
{
    public static string ReadWholeFile(string path)
    {
        var sr = new StreamReader(@$".\input\{path}");
        return sr.ReadToEnd().Trim();
    }

    public static IEnumerable<string> ReadLines(string path) =>
        ReadWholeFile(path).Split('\n').Select(i => i.Trim());
}
