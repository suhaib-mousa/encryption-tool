using System;

namespace EncryptionTool.Helpers;

public static class ConsoleHelper
{
    public static void WriteLineInColor(string message, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = originalColor;
    }
}
