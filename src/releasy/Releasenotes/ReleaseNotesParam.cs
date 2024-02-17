namespace tomware.Releasy.Releasenotes;

internal sealed record ReleaseNotesParam
(
  string Version,
  string PermaLink,
  string InputDirectory,
  string OutputDirectory,
  string? ArchiveDirectory
);