using System.Reflection;

namespace tomware.Releasy;

public static class Templates
{
  public const string ChangelogEntries = "ChangelogEntries";
  public const string ReleaseNotes = "ReleaseNotes";
}

public static class TemplateLoader
{
  public static string GetResource(string template)
  {
    var assembly = Assembly.GetExecutingAssembly();
    var resourcePath = assembly.ManifestModule.Name.Replace(".dll", string.Empty);
    var resourceName = $"{resourcePath}.Templates.{template}.liquid";

    using (var stream = assembly.GetManifestResourceStream(resourceName)!)
    using (var reader = new StreamReader(stream!))
    {
      return reader.ReadToEnd();
    }

    throw new FileNotFoundException($"Template with name '{template}' does not exist!");
  }
}
