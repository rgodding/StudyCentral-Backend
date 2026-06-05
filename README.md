# Study Central

## Setup user-secrets
```csharp
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=studycentraldb;User=root;Password=Skejs123;"
dotnet user-secrets set "Jwt:Key" "SZBNheG6DYChL2oyIo6Q3dAiK4sREZGPX6orWfH2Mk="
dotnet user-secrets set "Jwt:Audience" "StudyCentral.dk"
dotnet user-secrets set "Jwt:Issuer" "StudyCentral.dk"
dotnet user-secrets set "Azurite:ConnectionString" "UseDevelopmentStorage=true;"
dotnet user-secrets set "Azurite:UseHttps" "false"
dotnet user-secrets set "Azurite:Container" "studycentralcontainer"
```

## Start Azurite
```bash
azurite --silent --location azurite --debug azurite-debug.log
```

## Setup Coverage tool
```bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
start coveragereport/index.html
```

## Update EF Core migration
```bash
dotnet ef migrations remove
dotnet ef database drop
dotnet ef migrations add Init
dotnet ef database update
```
