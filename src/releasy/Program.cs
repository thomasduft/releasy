using McMaster.Extensions.CommandLineUtils;

using tomware.Releasy.Changelog;
using tomware.Releasy.Releasenotes;

using static tomware.Releasy.ConsoleHelper;

var app = new CommandLineApplication
{
  Name = "releasy"
};

app.HelpOption();

app.Command("add-changelog", (command) =>
{
  command.Description = "Creates a new changelog entry (i.e. releasy add-changelog -i \"my-issue-id\" -p \"feature\" -t \"audit\" -m \"My super duper text\")";
  var issueIdOption = command.Option("-i|--issue-id", "IssueId to link to your ticketing system", CommandOptionType.SingleValue);
  var prefixOption = command.Option("-p|--prefix", "Prefix like added, changed, deprecated, removed, fixed, security", CommandOptionType.SingleValue);
  var tagOption = command.Option("-t|--tag", "Tag that specifies the changelog entry. Helpful for grouping and such things.", CommandOptionType.SingleValue);
  var messageOption = command.Option("-m|--message", "Changelog text.", CommandOptionType.SingleValue);
  command.HelpOption();
  command.OnExecute(() =>
  {
    var issueId = issueIdOption.HasValue()
      ? issueIdOption.Value() ?? throw new InvalidOperationException(nameof(issueIdOption.Value))
      : ReadInput("Enter your IssueId");
    var prefix = prefixOption.HasValue()
      ? prefixOption.Value() ?? throw new InvalidOperationException(nameof(prefixOption.Value))
      : ReadInput("Enter prefix (i.e. added, changed, deprecated, fixed, removed, security)");
    var tag = tagOption.HasValue()
      ? tagOption.Value() ?? throw new InvalidOperationException(nameof(tagOption.Value))
      : ReadInput("Enter tag");
    var message = messageOption.HasValue()
      ? messageOption.Value() ?? throw new InvalidOperationException(nameof(messageOption.Value))
      : ReadInput("Enter message (can contain simple inline markdown)");

    var creator = new ChangelogCreator(
      new ChangelogParam(
        issueId,
        prefix,
        tag,
        message
      ));
    creator.Create();

    return 0;
  });
});

app.Command("add-releasenote", (command) =>
{
  command.Description = "Creates a new release note entry (i.e. releasy add-releasenote -i \"my-issue-id\" -p \"feature\" -t \"audit\" -m \"My super duper text\")";
  var issueIdOption = command.Option("-i|--issue-id", "IssueId to link to your ticketing system", CommandOptionType.SingleValue);
  var prefixOption = command.Option("-p|--prefix", "Prefix like breaking, deprecated, feature, fix, performance, removed, security, upgrade", CommandOptionType.SingleValue);
  var tagOption = command.Option("-t|--tag", "Tag that specifies the changelog entry (i.e. audit). Helpful for grouping and such things.", CommandOptionType.SingleValue);
  var messageOption = command.Option("-m|--message", "Release note text.", CommandOptionType.SingleValue);
  var instructionsOption = command.Option("-ins|--instructions", "Optional instructions for the release note message.", CommandOptionType.SingleValue);
  command.HelpOption();
  command.OnExecute(() =>
  {
    var issueId = issueIdOption.HasValue()
      ? issueIdOption.Value() ?? throw new InvalidOperationException(nameof(issueIdOption.Value))
      : ReadInput("Enter your IssueId");
    var prefix = prefixOption.HasValue()
      ? prefixOption.Value() ?? throw new InvalidOperationException(nameof(prefixOption.Value))
      : ReadInput("Enter prefix (i.e. breaking, deprecated, feature, fix, performance, removed, security, upgrade)");
    var tag = tagOption.HasValue()
      ? tagOption.Value() ?? throw new InvalidOperationException(nameof(tagOption.Value))
      : ReadInput("Enter tag");
    var message = messageOption.HasValue()
      ? messageOption.Value() ?? throw new InvalidOperationException(nameof(messageOption.Value))
      : ReadInput("Enter release note message (can contain simple inline markdown)");
    var instructions = instructionsOption.HasValue()
      ? [instructionsOption.Value() ?? throw new InvalidOperationException(nameof(instructionsOption.Value))]
      : ReadMultilineInput("Enter intstructions for the release note message (can contain simple inline markdown)");

    var creator = new ReleaseNoteCreator(
      new ReleaseNoteParam(
        issueId,
        prefix,
        tag,
        message,
        instructions
      ));
    creator.Create();

    return 0;
  });
});

app.Command("create-releasenotes", (command) =>
{
  command.Description = "Creates releasenotes based on release note entries for a dedicated release (i.e. releasy create-releasenotes -v \"1.2.3\" -p \"some-permalink\")";
  var versionOption = command.Option("-v|--version", "Release version", CommandOptionType.SingleValue);
  var permaLinkOption = command.Option("-p|--permaLink", "Permalink template that points to the VCS issue id", CommandOptionType.SingleValue);
  var inputDirectoryOption = command.Option("-i|--inputDirectory", "Input directory to scan for release note items (defaults to '.')", CommandOptionType.SingleValue);
  var outputDirectoryOption = command.Option("-o|--outputDirectory", "Output directory where a release note will be stored (defaults to '.')", CommandOptionType.SingleValue);
  var archiveDirectoryOption = command.Option("-a|--archiveDir", "Archive directory (if not set deletes existing release note entries)", CommandOptionType.SingleValue);
  command.HelpOption();
  command.OnExecute(() =>
  {
    var version = versionOption.HasValue()
      ? versionOption.Value() ?? throw new InvalidOperationException(nameof(versionOption.Value))
      : ReadInput("Enter release version");
    var permaLink = permaLinkOption.HasValue()
      ? permaLinkOption.Value() ?? throw new InvalidOperationException(nameof(permaLinkOption.Value))
      : ReadInput("Enter the permalink template that points to the VCS issue id");
    var inputDirectory = inputDirectoryOption.HasValue()
      ? inputDirectoryOption.Value() ?? throw new InvalidOperationException(nameof(inputDirectoryOption.Value))
      : ".";
    var outputDirectory = outputDirectoryOption.HasValue()
      ? outputDirectoryOption.Value() ?? throw new InvalidOperationException(nameof(outputDirectoryOption.Value))
      : ".";
    var archiveDirectory = archiveDirectoryOption.HasValue()
      ? archiveDirectoryOption.Value() ?? throw new InvalidOperationException(nameof(archiveDirectoryOption.Value))
      : null; // => dump them!

    var releaseNotes = new ReleaseNotes(
      new ReleaseNotesParam(
        version,
        permaLink,
        inputDirectory,
        outputDirectory,
        archiveDirectory
      ));
    releaseNotes.Create();

    return 0;
  });
});

app.Command("update-changelog", (command) =>
{
  // see keepachangelog.com (https://keepachangelog.com/en/1.0.0/#how)
  command.Description = "Updates the CHANGELOG.md based on changelog entries for a dedicated release (i.e. releasy update-changelog -v \"1.2.3\" -p \"some-permalink\")";
  var versionOption = command.Option("-v|--version", "Release version", CommandOptionType.SingleValue);
  var permaLinkOption = command.Option("-p|--permaLink", "Permalink template that points to the VCS issue id", CommandOptionType.SingleValue);
  var inputDirectoryOption = command.Option("-i|--inputDirectory", "Input directory to scan for release note items (defaults to '.')", CommandOptionType.SingleValue);
  var changelogFileNameOption = command.Option("-f|--file", "Changelog file to be processed (defaults to 'CHANGELOG.md')", CommandOptionType.SingleValue);
  command.HelpOption();
  command.OnExecute(() =>
  {
    var version = versionOption.HasValue()
      ? versionOption.Value() ?? throw new InvalidOperationException(nameof(versionOption.Value))
      : ReadInput("Enter release version");
    var permaLink = permaLinkOption.HasValue()
      ? permaLinkOption.Value() ?? throw new InvalidOperationException(nameof(permaLinkOption.Value))
      : ReadInput("Enter the permalink template that points to the VCS issue id");
    var inputDirectory = inputDirectoryOption.HasValue()
      ? inputDirectoryOption.Value() ?? throw new InvalidOperationException(nameof(inputDirectoryOption.Value))
      : ".";
    var changelogFileName = changelogFileNameOption.HasValue()
      ? changelogFileNameOption.Value() ?? throw new InvalidOperationException(nameof(changelogFileNameOption.Value))
      : "CHANGELOG.md";

    var changelogUpdater = new ChangelogUpdater(new ChangelogUpdaterParam(
          version,
          permaLink,
          inputDirectory,
          changelogFileName
        ));
    changelogUpdater.UpdateChangelog();

    return 0;
  });
});

app.OnExecute(() =>
{
  app.ShowHelp();

  return 0;
});

return app.Execute(args);