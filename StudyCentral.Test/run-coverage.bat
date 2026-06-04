@echo off

echo Running tests with coverage...
dotnet test --collect:"XPlat Code Coverage"

echo Generating HTML report...
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

echo Opening coverage report...
start coveragereport/index.html

echo Done.
pause