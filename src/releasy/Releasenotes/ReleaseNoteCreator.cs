using System.Text.Json;

namespace tomware.Releasy.Releasenotes;

internal sealed class ReleaseNoteCreator
{
  private readonly ReleaseNoteParam _releaseNoteParam;
  private readonly JsonSerializerOptions _jsonSerializerOptions = new()
  {
    PropertyNameCaseInsensitive = true,
    WriteIndented = true
  };

  public ReleaseNoteCreator(ReleaseNoteParam changelogParam)
  {
    _releaseNoteParam = changelogParam;
  }

  public void Create()
  {
    var releaseNoteEntry = ReleaseNoteEntry.Create(
      _releaseNoteParam.IssueId,
      _releaseNoteParam.Prefix,
      _releaseNoteParam.Tag,
      _releaseNoteParam.Message,
      _releaseNoteParam.Instructions
    );

    // Guid ToString(...) formats (https://learn.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=net-6.0)
    var fileName = releaseNoteEntry.Id.ToString("N");
    var path = $"{fileName}.{Constants.ReleaseNoteEntryFileExtension}";

    var content = JsonSerializer.Serialize(releaseNoteEntry, _jsonSerializerOptions);
    File.WriteAllText(path, content);
  }
}