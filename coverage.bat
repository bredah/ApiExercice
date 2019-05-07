@echo off
set /p token=<sonar.txt
dotnet sonarscanner begin /k:"bredah_csharp-webapi" /n:"WebApi" /v:"1.0" /o:"bredah-github" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="%token%" /d:sonar.language="cs"
dotnet restore
dotnet build
dotnet test WebApi.Tests/*.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=../lcov
dotnet test WebApi.Integration.Tests/*.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=../lcov
dotnet sonarscanner end /d:sonar.login="%token%"