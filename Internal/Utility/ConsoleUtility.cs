namespace ChroniclesExporter.Utility;

public static class ConsoleUtility
{
    /// <summary>
    ///     Prints a confirmation prompt to the console
    /// </summary>
    /// <param name="pMessage">String message to print preceding the prompt</param>
    public static bool ConfirmPrompt(string pMessage)
    {
        ConsoleKey response;
        Console.WriteLine($"{pMessage} [y/n]");
        do
        {
            response = Console.ReadKey(true).Key;
        } while (response != ConsoleKey.Y && response != ConsoleKey.N);

        return response == ConsoleKey.Y;
    }

    #region --- Marked Lines ---

    /// <summary>
    ///     Writes a marked string to the console
    /// </summary>
    public static void WriteMarked(string pMessage, EConsoleMark pMark)
    {
        Console.Write('[');
        ConsoleColor color = Console.ForegroundColor;
        Console.ForegroundColor = GetMarkColor(pMark);
        Console.Write(GetMarkChar(pMark));
        Console.ForegroundColor = color;
        Console.Write($"] {pMessage}");
    }

    /// <summary>
    ///     Writes a marked line to the console
    /// </summary>
    public static void WriteMarkedLine(string pMessage, EConsoleMark pMark)
    {
        WriteMarked(pMessage, pMark);
        Console.Write(Environment.NewLine);
    }

    private static char GetMarkChar(EConsoleMark pMark)
    {
        return pMark switch
        {
            EConsoleMark.Check => '✓',
            EConsoleMark.Info => 'i',
            EConsoleMark.Warning => '!',
            EConsoleMark.Error => 'x',
            _ => throw new ArgumentOutOfRangeException($"No Mark defined for [{pMark}]")
        };
    }

    private static ConsoleColor GetMarkColor(EConsoleMark pMark)
    {
        return pMark switch
        {
            EConsoleMark.Check => ConsoleColor.Green,
            EConsoleMark.Info => ConsoleColor.Gray,
            EConsoleMark.Warning => ConsoleColor.Yellow,
            EConsoleMark.Error => ConsoleColor.Red,
            _ => throw new ArgumentOutOfRangeException($"No Color defined for [{pMark}]")
        };
    }

    #endregion
}

[Flags]
public enum EConsoleMark
{
    Info = 1 << 0,
    Warning = 1 << 1,
    Error = 1 << 2,
    Check = 1 << 3
}
