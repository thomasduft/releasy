namespace tomware.Releasy.Changelog;

internal sealed record ChangelogParam
(
  string IssueId,
  string Prefix,
  string Tag,
  string Message
);