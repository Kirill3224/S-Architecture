

using SL.Domain.Common;

namespace SL.Cli;

public static class MinorMethods
{
    public static void PrintHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"> {title} <");
        Console.ResetColor();
    }

    public static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n[Error] {message}");
        Console.ResetColor();

        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadKey();
    }

    public static void ShowSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n[Success] {message}");
        Console.ResetColor();

        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadKey();
    }
}