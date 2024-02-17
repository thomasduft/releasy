namespace tomware.Releasy.Releasenotes;

internal sealed record ReleaseNoteParam
(
  string IssueId,
  string Prefix,
  string Tag,
  string Message,
  string[] Instructions
);