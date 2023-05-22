using System.Globalization;

namespace tomware.Releasy;

public static class StringExtensions
{
  public static string UpperCaseFirstLetter(this string input)
  {
    if (string.IsNullOrEmpty(input))
      return input;

    return input[..1].ToUpper(CultureInfo.CurrentCulture) + input[1..];
  }
}