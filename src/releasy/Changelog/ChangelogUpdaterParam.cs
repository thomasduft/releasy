namespace tomware.Releasy;

internal sealed record ChangelogUpdaterParam
(
  string Version,
  string PermaLink,
  string InputDirectory,
  string ChangelogFileName
);