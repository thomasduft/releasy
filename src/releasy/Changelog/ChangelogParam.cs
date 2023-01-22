namespace tomware.Releasy;

internal sealed record ChangelogParam
(
  string IssueId,
  string Prefix,
  string Tag,
  string Message
);
