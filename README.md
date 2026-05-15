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
