# Reg.Roup
Regex Group Deserializer

## Configuring Package Source
As described [here](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry#publishing-a-package) in the GitHub docs, run the following command to configure the source for this package, replacing `<YOUR_GITHUB_USERNAME>` and `<YOUR_GITHUB_PAT>` with your username and Personal Access Token repsectively.

    dotnet nuget add source --username <YOUR_GITHUB_USERNAME> --password <YOUR_GITHUB_PAT> --store-password-in-clear-text --name thick_prophet "https://nuget.pkg.github.com/ThickPropheT/index.json"
