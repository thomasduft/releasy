using System.Text.Json;

namespace tomware.Releasy.Changelog;

internal sealed class ChangelogCreator
{
  private readonly ChangelogParam _changelogParam;
  private readonly JsonSerializerOptions _jsonSerializerOptions = new()
  {
    PropertyNameCaseInsensitive = true,
    WriteIndented = true
  };

  public ChangelogCreator(ChangelogParam changelogParam)
  {
    _changelogParam = changelogParam;
  }

  public void Create()
  {
    var changelog = ChangelogEntry.Create(
      _changelogParam.IssueId,
      _changelogParam.Prefix,
      _changelogParam.Tag,
      _changelogParam.Message
    );

    // Guid ToString(...) formats (https://learn.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=net-6.0)
    var fileName = changelog.Id.ToString("N");
    var path = $"{fileName}.{Constants.ChangelogEntryFileExtension}";

    var content = JsonSerializer.Serialize(changelog, _jsonSerializerOptions);
    File.WriteAllText(path, content);
  }
}