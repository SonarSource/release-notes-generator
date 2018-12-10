# Description
Release notes generator for SonarSource projects hosted on GitHub

# Use

Visit:
```
https://sonar-dotnet-rel-notes.azurewebsites.net/api/gen?r=<repository>&m=<milestone>&t=<token>
```

Replace:
- `repository` with a repository name, including owner, for example `SonarSource/sonar-dotnet`
- `milestone` with a milestone name, for example `7.10`
- `token` GitHub token that has read access to the repository

### Example
```
https://sonar-dotnet-rel-notes.azurewebsites.net/api/gen?r=SonarSource/sonar-dotnet&m=7.10&t=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
```

### Notes

Currently works with the SonarC#/VB.NET and SonarLint for Visual Studio labels

# Future

Store the GitHub token on Azure, to avoid the need to provide it through the url
