using System.Text.Json;

using Fluid;

namespace tomware.Releasy.Releasenotes;

internal sealed class ReleaseNotes
{
  private readonly List<ReleaseNoteEntry> _releaseNotes;
  private readonly ReleaseNotesParam _releaseNotesParam;

  public ReleaseNotes(ReleaseNotesParam releaseNotesParam)
  {
    _releaseNotes = [];
    _releaseNotesParam = releaseNotesParam;
  }

  public void Create()
  {
    // 1. Find all changelog entries
    var files = GetFiles(_releaseNotesParam.InputDirectory);
    foreach (var file in files)
    {
      var content = File.ReadAllText(file);
      var note = JsonSerializer.Deserialize<ReleaseNoteEntry>(content);
      if (note != null)
        _releaseNotes.Add(note);
    }

    var outputDirectory = _releaseNotesParam.OutputDirectory;

    // 2. Store ReleaseNotes.md
    // TODO: do not allow dots in version string
    var releaseNotesPath = $"{outputDirectory}/ReleaseNotes_{_releaseNotesParam.Version}.md";
    SaveFile(
      releaseNotesPath,
      new
      {
        _releaseNotesParam.Version,
        _releaseNotesParam.PermaLink,
        Prefixes = BuildPrefixes(_releaseNotes)
      }
    );

    // 3. Archive or delete changelog files
    if (_releaseNotesParam.ArchiveDirectory is not null)
    {
      var archive = $"{outputDirectory}/{_releaseNotesParam.ArchiveDirectory}/{_releaseNotesParam.Version}";
      Directory.CreateDirectory(archive);
      foreach (var file in files)
      {
        var info = new FileInfo(file);
        File.Move(file, $"{archive}/{info.Name}", true);
      }
    }
    else
    {
      foreach (var file in files)
      {
        File.Delete(file);
      }
    }
  }

  private static List<string> GetFiles(string inputDirectory)
  {
    var files = new List<string>();

    files.AddRange(Directory.GetFiles(
      inputDirectory,
      $"*.{Constants.ReleaseNoteEntryFileExtension}",
      SearchOption.AllDirectories
    ));

    return files;
  }

  private static IEnumerable<Prefix> BuildPrefixes(IEnumerable<ReleaseNoteEntry> releaseNotes)
  {
    var prefixes = new List<Prefix>();

    var prefixGroups = releaseNotes.Select(r => r.Prefix).Distinct();
    foreach (var prefix in prefixGroups)
    {
      var notes = releaseNotes
        .Where(r => r.Prefix == prefix)
        .OrderBy(r => r.CreatedAt);

      prefixes.Add(new Prefix(prefix, notes));
    }

    return prefixes.OrderBy(p => p.Name);
  }

  private static void SaveFile(string output, object model)
  {
    var source = TemplateLoader.GetResource(Templates.ReleaseNotes);
    var template = new FluidParser().Parse(source);

    var options = new TemplateOptions();
    options.MemberAccessStrategy.Register<Prefix>();
    options.MemberAccessStrategy.Register<ReleaseNoteEntry>();

    var content = template.Render(new TemplateContext(model, options));
    File.WriteAllText(output, content);
  }

  private sealed record Prefix
  (
    string Name,
    IEnumerable<ReleaseNoteEntry> Entries
  );
}