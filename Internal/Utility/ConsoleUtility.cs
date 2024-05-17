namespace ChroniclesExporter.Utility;

public class ConsoleUtility
{
    /// <summary>
    /// Writes a marked string to the console
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
    /// Writes a marked line to the console
    /// </summary>
    public static void WriteMarkedLine(string pMessage, EConsoleMark pMark)
    {
        WriteMarked(pMessage, pMark);
        Console.Write(Environment.NewLine);
    }

    /// <summary>
    /// Overwrites a line with a new line in the console
    /// </summary>
    /// <param name="pCursorTop">Index of the line to overwrite</param>
    public static void OverwriteLine(string pMessage, int pCursorTop)
    {
        int line = Console.CursorTop;
        Console.SetCursorPosition(0, pCursorTop);
        Console.Write(pMessage);
        Console.Write(new string(' ', Console.BufferWidth - Console.CursorLeft - 1));
        Console.SetCursorPosition(0, line);
    }
    /// <summary>
    /// Overwrites the last line with a new line in the console
    /// </summary>
    public static void OverwriteLine(string pMessage, EConsoleMark pMark) =>
        OverwriteLine(pMessage, Console.CursorTop - 1);
    
    /// <summary>
    /// Overwrites a line with a new marked line in the console
    /// </summary>
    /// <param name="pCursorTop">Index of the line to overwrite</param>
    public static void OverwriteMarkedLine(string pMessage, EConsoleMark pMark, int pCursorTop)
    {
        int line = Console.CursorTop;
        Console.SetCursorPosition(0, pCursorTop);
        WriteMarked(pMessage, pMark);
        Console.Write(new string(' ', Console.BufferWidth - Console.CursorLeft - 1));
        Console.SetCursorPosition(0, line);
    }
    /// <summary>
    /// Overwrites a line with a new marked line in the console
    /// </summary>
    public static void OverwriteMarkedLine(string pMessage, EConsoleMark pMark) =>
        OverwriteMarkedLine(pMessage, pMark, Console.CursorTop - 1);

    private static char GetMarkChar(EConsoleMark pMark)
    {
        return pMark switch
        {
            EConsoleMark.Check => '✓',
            EConsoleMark.Warning => '!',
            EConsoleMark.Error => 'x',
            EConsoleMark.Waiting => GetWaitingChar(),
            _ => throw new ArgumentOutOfRangeException($"No Mark defined for [{pMark}]")
        };
    }

    private static char GetWaitingChar()
    {
        return DateTime.Now.Millisecond switch
        {
            < 250 => '-',
            < 500 => '\\',
            < 750 => '|',
            _ => '/'
        };
    }

    private static ConsoleColor GetMarkColor(EConsoleMark pMark)
    {
        return pMark switch
        {
            EConsoleMark.Check => ConsoleColor.Green,
            EConsoleMark.Warning => ConsoleColor.Yellow,
            EConsoleMark.Error => ConsoleColor.Red,
            EConsoleMark.Waiting => ConsoleColor.Gray,
            _ => throw new ArgumentOutOfRangeException($"No Color defined for [{pMark}]")
        };
    }
}

public enum EConsoleMark
{
    Check,
    Warning,
    Error,
    Waiting
}