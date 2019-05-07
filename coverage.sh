#!/bin/bash
token="$(cat sonar.txt)"
dir="$(pwd)"
dotnet sonarscanner begin /k:"bredah_csharp-webapi" /n:"WebApi" /v:"1.0" /o:"bredah-github" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="${token}" /d:sonar.language="cs" /d:sonar.exclusions="**/bin/**/*,**/obj/**/*" /d:sonar.cs.opencover.reportsPaths="${dir}/lcov.opencover.xml"
dotnet restore
dotnet build
dotnet test Project.Tests/Project.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=../lcov
dotnet sonarscanner end /d:sonar.login="${token}"