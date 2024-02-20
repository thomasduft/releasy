![build](https://github.com/thomasduft/releasy/actions/workflows/build.yml/badge.svg) [![NuGet](https://img.shields.io/nuget/vpre/tomware.Releasy.svg)](https://www.nuget.org/packages/tomware.Releasy)

# releasy

releasy - a simple release notes tool.

## What is the tool for?

Writing and maintaining changelogs or release notes is not something a developer likes to spend time for nor is it fun to gather every information for a feature right before release time and write your changelogs or release notes late after you implemented a particular feature.

`releasy` offers a pragmatic but opinionated approach to provide changelog or release note entries within a pull-request (PR), store them until release date and generate the artifacts as a part of your build process when your about to release a new version.

The `releasy`-tool allows the following processes:

- **Adding a changelog entry**: Provided by the developer within a PR.
- **Adding a release note entry**: Provided by the developer within a PR.
- **Creating release notes**: Created out of existing release note entries within the repository as a part of the build pipeline.
- **Updating a `CHANGELOG.md`-file**: Created out of existing changelog entries within the repository as a part of the build pipeline.

## How to use

### List all arguments

> releasy -h

```console
Usage: releasy [command] [options]

Options:
  -?|-h|--help         Show help information.

Commands:
  add-changelog        Creates a new changelog entry (i.e. releasy add-changelog -i "my-issue-id" -p "feature" -t "audit" -m "My super duper text")
  add-releasenote      Creates a new release note entry (i.e. releasy add-releasenote -i "my-issue-id" -p "feature" -t "audit" -m "My super duper text")
  create-releasenotes  Creates releasenotes based on release note entries for a dedicated release (i.e. releasy create-releasenotes -v "1.2.3" -p "some-permalink")
  update-changelog     Updates the CHANGELOG.md based on changelog entries for a dedicated release (i.e. releasy update-changelog -v "1.2.3" -p "some-permalink")

Run 'releasy [command] -?|-h|--help' for more information about a command.
```
