# C# WebApi

Example of a web API and all test phases.

[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=bredah_csharp-webapi)](https://sonarcloud.io/dashboard?id=bredah_csharp-webapi)

[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=bredah_csharp-webapi&metric=code_smells)](https://sonarcloud.io/dashboard?id=bredah_csharp-webapi)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=bredah_csharp-webapi&metric=bugs)](https://sonarcloud.io/dashboard?id=bredah_csharp-webapi)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=bredah_csharp-webapi&metric=coverage)](https://sonarcloud.io/dashboard?id=bredah_csharp-webapi)

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