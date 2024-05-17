namespace ChroniclesExporter.Utility;

public class ConsoleUtility
{
    /// <summary>
    /// Prints a confirmation prompt to the console
    /// </summary>
    /// <param name="pMessage">String message to print preceding the prompt</param>
    /// <param name="pShowInput">Show any key input provided</param>
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
    
    /// <summary>
    /// Prints a confirmation prompt to the console
    /// </summary>
    /// <param name="pMessage">String message to print preceding the prompt</param>
    /// <param name="pOptions">Show any key input provided</param>
    public static int ComplexPrompt(string pMessage, string[] pOptions)
    {
        if (pOptions.Length <= 0)
            throw new ArgumentNullException(nameof(pOptions), "No Options provided in a complex-prompt");
        
        Console.WriteLine($"{pMessage}");
        for (int i = 0; i < pOptions.Length; i++)
        {
            Console.WriteLine($" [{i + 1}] {pOptions[i]}");
        }
        if (pOptions.Length >= 10) 
            WriteMarkedLine($"{pOptions.Length - 9} options were truncated", EConsoleMark.Warning);
        
        while (true)
        {
            if (int.TryParse(Console.ReadKey().ToString(), out int number) && number > 0 && number <= pOptions.Length)
                return number - 1;
        }
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

    #region --- Marked Lines ---

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
            EConsoleMark.Info => 'i',
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
            EConsoleMark.Info => ConsoleColor.Cyan,
            EConsoleMark.Warning => ConsoleColor.Yellow,
            EConsoleMark.Error => ConsoleColor.Red,
            EConsoleMark.Waiting => ConsoleColor.Gray,
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
    Check = 1 << 3,
    Waiting = 1 << 4
}