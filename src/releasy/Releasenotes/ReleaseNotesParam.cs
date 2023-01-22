namespace tomware.Releasy;

internal sealed record ReleaseNotesParam
(
  string Version,
  string PermaLink,
  string InputDirectory,
  string OutputDirectory,
  string? ArchiveDirectory
);
