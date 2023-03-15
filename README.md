![build](https://github.com/thomasduft/releasy/actions/workflows/build.yml/badge.svg) [![NuGet](https://img.shields.io/nuget/vpre/tomware.Releasy.svg)](https://www.nuget.org/packages/tomware.Releasy)

# releasy

releasy - a simple release notes tool.

## How to use

### List all arguments

> releasy -h

```console
Usage: releasy [command] [options]

Options:
  -?|-h|--help         Show help information.

Commands:
  add-changelog        Creates a new changelog entry (i.e. releasy add-changelog -i "my-issue-id" -p "feature" -t "audit" -m "My super duper text")
  create-releasenotes  Creates releasenotes based on changelog entries for a dedicated release (i.e. releasy create-releasenotes -v "1.2.3" -p "some-perma-link")
  update-changelog     Updates the CHANGELOG.md based on changelog entries for a dedicated release (i.e. releasy update-changelog -v "1.2.3" -p "some-perma-link")

Run 'releasy [command] -?|-h|--help' for more information about a command.
```
