namespace tomware.Releasy.Changelog;

internal sealed class ChangelogEntry
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string IssueId { get; set; } = string.Empty;
  public string Prefix { get; set; } = string.Empty;
  public string Tag { get; set; } = string.Empty;
  public string Message { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; } = DateTime.Now;
  public string CreatedBy { get; set; } = UserNameProvider.GetUserName();

  public static ChangelogEntry Create(
    string issueId,
    string prefix,
    string tag,
    string message
  )
  {
    return new ChangelogEntry
    {
      IssueId = issueId,
      Prefix = prefix.UpperCaseFirstLetter(),
      Tag = tag,
      Message = message
    };
  }
}