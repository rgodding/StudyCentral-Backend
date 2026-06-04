@echo off

rmdir /s /q TestResults 2>nul
rmdir /s /q coveragereport 2>nul

echo Running tests with coverage...
dotnet test --collect:"XPlat Code Coverage"

echo Generating HTML report...
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

echo Opening coverage report...
start coveragereport/index.html

pause