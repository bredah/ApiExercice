# C# WebApi

Example of a web API and all test phases, based in the guidelines:

- [Microsoft - API](https://github.com/Microsoft/api-guidelines/blob/master/Guidelines.md)

Project Status:

[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=bredah_csharp-webapi)](https://sonarcloud.io/dashboard?id=bredah_csharp-webapi)

[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=bredah_csharp-webapi&metric=code_smells)](https://sonarcloud.io/dashboard?id=bredah_csharp-webapi)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=bredah_csharp-webapi&metric=bugs)](https://sonarcloud.io/dashboard?id=bredah_csharp-webapi)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=bredah_csharp-webapi&metric=coverage)](https://sonarcloud.io/dashboard?id=bredah_csharp-webapi)

[![Build Status](https://dev.azure.com/bredah/CSharp-WebApi/_apis/build/status/bredah.csharp-webapi?branchName=master)](https://dev.azure.com/bredah/CSharp-WebApi/_build/latest?definitionId=1&branchName=master)

## Project Structure

- WebApi: Base Project
- WebApi.Tests: Unit test
- WebApi.Integration.Tests: Testes integrados

## CLI

- Clean the solution

```bash
dotnet clean
```

- Compile the solution

```bash
dotnet build
```

- Running all tests

```bash
dotnet test
```

- Run the application

```bash
dotnet run --project./APPLICATION
```

- Running all tests and generate a coverage report

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info
```

- Enable the coverage in real time by the `dotnet watch`

```bash
dotnet watch --project ./PROJECT test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info
```

## EF - DB Seed

Create a initial state

```bash
dotnet ef migrations add Initial --project WebApi
```

Drop the old information

```bash
dotnet ef database drop --force --project WebApi
```

Load data into db

```bash
dotnet ef database update --project WebApi
```

## Code Inspection - Local

Install the Sonar Scanner plugin

```bash
dotnet tool install --global dotnet-sonarscanner
```

Add the token in the file `sonar.txt` and run the script occording to the OS.

## Setup

### Visual Studio Code

Plugins:

- C# for Visual Studio Code (powered by OmniSharp)
- C# Extensions
- C# FixFormat
- C# Snippets
- C# XML Documentation Comments
- C# IL Viewer
- Coverage Gutters
- Dotnet Core Essentials
- Path Intellisense
- SQLTools
- Super Sharp (C# extensions)
- vscode-solution-explorer
- .NET Core Test Explorer
- .NET Core Extension Pack