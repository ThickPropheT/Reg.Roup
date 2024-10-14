# Reg.Roup
Regex Group Deserializer

## Configuring Package Source
As described in the [GitHub docs][1], run the following command to configure the source for this package, replacing `<YOUR_GITHUB_USERNAME>` and `<YOUR_GITHUB_PAT>` with your username and Personal Access Token repsectively.

    dotnet nuget add source --username <YOUR_GITHUB_USERNAME> --password <YOUR_GITHUB_PAT> --store-password-in-clear-text --name thick_prophet "https://nuget.pkg.github.com/ThickPropheT/index.json"

## Releases

All published packages can be found on the repo's [Packages page][2].

All publisehd release can be found on the repo's [Releases page][3].

### Publishing
Both releases and prereleases are published by pushing `git tag`s.

#### Table 1 Release stage required inputs & expected outputs
|    Stage   | Branch | Tests Passing | `tagname` Format | Example                 | Artifacts                               |
|:----------:|:------:|:-------------:|:-----------------|:------------------------|:---------------------------------------:|
| Prerelease | `*`    | No            | `v#.#.#-pre###`  | `git tag v1.0.2-pre001` | [`*.nupkg`][2]                          |
|   Release  | `main` | Yes           | `v#.#.#`         | `git tag v1.0.2`        | [`*.nupkg`][2], [Release][3], Changelog |

#### Steps
For the desired release stage from the table above:

1. Ensure that candidate release commit exists on the branch specified.
2. Ensure that all tests are passing if specified.
3. Tag the candidate commit using the `git tag` command, providing a `<tagname>` that matches the format specified.
4. Begin the release process by pushing the tag using the `git push --tags` command.
5. Find [package][1] and [release][2] artifacts published to their respective locations as specified.

[1]: https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry#publishing-a-package
[2]: https://github.com/ThickPropheT/Reg.Roup/pkgs/nuget/Reg.Roup
[3]: https://github.com/ThickPropheT/Reg.Roup/releases
