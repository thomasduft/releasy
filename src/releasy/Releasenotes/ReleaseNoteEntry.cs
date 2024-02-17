namespace tomware.Releasy.Releasenotes;

internal sealed class ReleaseNoteEntry
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string IssueId { get; set; } = string.Empty;
  public string Prefix { get; set; } = string.Empty;
  public string Tag { get; set; } = string.Empty;
  public string Message { get; set; } = string.Empty;
  public string[] Instructions { get; set; } = [];
  public DateTime CreatedAt { get; set; } = DateTime.Now;
  public string CreatedBy { get; set; } = Environment.UserName;

  public static ReleaseNoteEntry Create(
    string issueId,
    string prefix,
    string tag,
    string message,
    string[] instructions
  )
  {
    return new ReleaseNoteEntry
    {
      IssueId = issueId,
      Prefix = prefix.UpperCaseFirstLetter(),
      Tag = tag,
      Message = message,
      Instructions = instructions
    };
  }
}