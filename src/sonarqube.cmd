dotnet sonarscanner begin /k:"MiddlinkProjectKey" /d:sonar.host.url="http://localhost:9012" /d:sonar.login="6a2620e134ed2cf1d3c4952e08f1e33fe1959d33"
dotnet build
dotnet sonarscanner end /d:sonar.login="6a2620e134ed2cf1d3c4952e08f1e33fe1959d33"