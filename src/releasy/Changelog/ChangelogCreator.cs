using System.Text.Json;

namespace tomware.Releasy;

internal sealed class ChangelogCreator
{
  private readonly ChangelogParam _changelogParam;

  public ChangelogCreator
  (
    ChangelogParam changelogParam
  )
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

    var content = JsonSerializer.Serialize(
      changelog,
      new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
      });

    // Guid ToString(...) formats (https://learn.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=net-6.0)
    var fileName = changelog.Id.ToString("N");
    var path = $"{fileName}.{Constants.ChangelogEntryFileExtension}";
    File.WriteAllText(path, content);
  }
}