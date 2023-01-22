using System.Text;

namespace tomware.Releasy;

public static class ConsoleHelper
{
  public static void Exit(string reason)
  {
    WriteLineError(reason);
    Environment.Exit(1);
  }

  public static void WriteYellow(string value)
  {
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write(value);
    Console.ForegroundColor = ConsoleColor.White;
  }

  public static void WriteLineSuccess(string value)
  {
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(value);
    Console.ForegroundColor = ConsoleColor.White;
  }

  public static void WriteLineError(string value)
  {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(value);
    Console.ForegroundColor = ConsoleColor.White;
  }

  public static void WriteLine(string value)
  {
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(value);
  }

  public static string ReadInput(string prompt)
  {
    WriteYellow($"{prompt}: ");

    var input = Console.ReadLine();

    return !string.IsNullOrWhiteSpace(input)
      ? input
      : string.Empty;
  }

  public static string ReadPassword()
  {
    WriteYellow("Please enter a password: ");

    var cursorStartPosition = Console.CursorLeft;
    var passwordBuilder = new StringBuilder();
    var continueReading = true;
    while (continueReading)
    {
      var consoleKeyInfo = Console.ReadKey(intercept: true);
      if (consoleKeyInfo.Key == ConsoleKey.Enter)
      {
        continueReading = false;
        Console.Write(Environment.NewLine);
      }
      else if (consoleKeyInfo.Key == ConsoleKey.Backspace)
      {
        ApplyBackspace(passwordBuilder, cursorStartPosition);
      }
      else if (KeyIsNoArrow(consoleKeyInfo))
      {
        ApplyChar(passwordBuilder, consoleKeyInfo);
      }
    }

    return passwordBuilder.ToString();
  }

  private static void ApplyBackspace(StringBuilder passwordBuilder, int cursorStartPosition)
  {
    if (passwordBuilder.Length > 0 && Console.CursorLeft > cursorStartPosition)
    {
      passwordBuilder.Remove(passwordBuilder.Length - 1, 1);
      Console.Write("\b \b");
    }
  }

  private static bool KeyIsNoArrow(ConsoleKeyInfo consoleKeyInfo)
  {
    return consoleKeyInfo.Key is not ConsoleKey.LeftArrow and
        not ConsoleKey.UpArrow and
        not ConsoleKey.RightArrow and
        not ConsoleKey.DownArrow;
  }

  private static void ApplyChar(StringBuilder passwordBuilder, ConsoleKeyInfo consoleKeyInfo)
  {
    var passwordChar = consoleKeyInfo.KeyChar;
    passwordBuilder.Append(passwordChar);
    Console.Write("*");
  }
}
