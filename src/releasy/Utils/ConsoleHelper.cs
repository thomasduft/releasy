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

  public static string[] ReadMultilineInput(string prompt)
  {
    WriteYellow($"{prompt}: " + Environment.NewLine);

    var lines = new List<string>();

    string? line;
    while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
    {
      lines.Add(line + Environment.NewLine);
    }

    return lines.ToArray();
  }
}