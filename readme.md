# C# WebApi


Example of a web API and all test phases, based in the guidelines:

- [Microsoft - API](https://github.com/Microsoft/api-guidelines/blob/master/Guidelines.md)

- Project Status:

[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=bredah_csharp-webapi)](https://sonarcloud.io/dashboard?id=bredah_csharp-webapi)

[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=bredah_csharp-webapi&metric=code_smells)](https://sonarcloud.io/dashboard?id=bredah_csharp-webapi)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=bredah_csharp-webapi&metric=bugs)](https://sonarcloud.io/dashboard?id=bredah_csharp-webapi)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=bredah_csharp-webapi&metric=coverage)](https://sonarcloud.io/dashboard?id=bredah_csharp-webapi)


[![Build Status](https://dev.azure.com/bredah/CSharp-WebApi/_apis/build/status/bredah.csharp-webapi?branchName=master)](https://dev.azure.com/bredah/CSharp-WebApi/_build/latest?definitionId=1&branchName=master)

## Project Structure

- WebApi: Base Project
- WebApi.Tests: Unit test
- WebApi.Integration.Tests: Testes integrados

## Code Inspection - Local

Install the Sonar Scanner plugin

```bash
dotnet tool install --global dotnet-sonarscanner
```

Add the token in the file `sonar.txt` and run the script occording to the OS.