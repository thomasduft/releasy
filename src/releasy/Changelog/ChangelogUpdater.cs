using System.Globalization;
using System.Text.Json;

using Fluid;

using tomware.Releasy;

internal sealed class ChangelogUpdater
{
  private readonly List<ChangelogEntry> _changelogEntries;
  private readonly ChangelogUpdaterParam _changelogUpdaterParam;

  public ChangelogUpdater(
    ChangelogUpdaterParam changelogUpdaterParam
  )
  {
    _changelogEntries = new List<ChangelogEntry>();
    _changelogUpdaterParam = changelogUpdaterParam;
  }

  public void UpdateChangelog()
  {
    // 1. Find all changelog entries
    var files = GetFiles(_changelogUpdaterParam.InputDirectory);
    foreach (var file in files)
    {
      var content = File.ReadAllText(file);
      var entry = JsonSerializer.Deserialize<ChangelogEntry>(content);
      if (entry != null)
        _changelogEntries.Add(entry);
    }

    CheckChanglogExistsIfNotScaffoldOne(_changelogUpdaterParam.ChangelogFileName);

    InsertInChangelog(
      _changelogUpdaterParam.ChangelogFileName,
      new
      {
        _changelogUpdaterParam.Version,
        Date = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture),
        _changelogUpdaterParam.PermaLink,
        Prefixes = BuildPrefixes(_changelogEntries)
      }
    );

    // 3. delete changelog files
    foreach (var file in files)
    {
      File.Delete(file);
    }
  }

  private static IEnumerable<string> GetFiles(string inputDirectory)
  {
    var files = new List<string>();

    files.AddRange(Directory.GetFiles(
      inputDirectory,
      $"*.{Constants.ChangelogEntryFileExtension}",
      SearchOption.AllDirectories
    ));

    return files;
  }

  private static void CheckChanglogExistsIfNotScaffoldOne(string changelogFileName)
  {
    if (File.Exists(changelogFileName)) return;

    var content = TemplateLoader.GetResource(Templates.ChangelogTemplate);

    File.WriteAllText(changelogFileName, content);
  }

  private static void InsertInChangelog(string changelogFileName, object model)
  {
    var source = TemplateLoader.GetResource(Templates.ChangelogEntries);
    var template = new FluidParser().Parse(source);

    var options = new TemplateOptions();
    options.MemberAccessStrategy.Register<Prefix>();
    options.MemberAccessStrategy.Register<ChangelogEntry>();

    var content = template.Render(new TemplateContext(model, options));

    var lines = File
      .ReadAllLines(changelogFileName)
      .ToList();
    lines.Insert(1, content);

    File.WriteAllLines(changelogFileName, lines);
  }

  private static IEnumerable<Prefix> BuildPrefixes(IEnumerable<ChangelogEntry> releaseNotes)
  {
    var prefixes = new List<Prefix>();

    var prefixGroups = releaseNotes.Select(r => r.Prefix).Distinct();
    foreach (var prefix in prefixGroups)
    {
      var notes = releaseNotes.Where(r => r.Prefix == prefix).OrderBy(r => r.CreatedAt);

      prefixes.Add(new Prefix(prefix, notes));
    }

    return prefixes.OrderBy(p => p.Name);
  }

  private sealed record Prefix
  (
    string Name,
    IEnumerable<ChangelogEntry> Entries
  );
}
