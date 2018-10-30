# Description
Release notes generator for SonarSource projects hosted on GitHub

# Use
dotnet ReleaseNotes.dll <owner/repo> <milestone-name> <github-token>

### Example
dotnet ReleaseNotes.dll SonarSource/sonar-csharp 7.8 <github-token>

### Notes

Currently works with the SonarC#/VB.NET and SonarLint for Visual Studio labels

# Future

Create an Azure function to execute without the need to compile
