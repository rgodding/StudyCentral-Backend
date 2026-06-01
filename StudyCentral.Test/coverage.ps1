Write-Host "Running tests with coverage..."

dotnet test /p:CollectCoverage=true /p:CoverletOutput=.\Coverage\coverage /p:CoverletOutputFormat=cobertura

if ($LASTEXITCODE -ne 0) {
Write-Host "Tests failed." -ForegroundColor Red
exit 1
}

Write-Host "Generating report..."

reportgenerator -reports:".\Coverage\coverage.cobertura.xml" -targetdir:".\coveragereport"

if ($LASTEXITCODE -ne 0) {
Write-Host "Report generation failed." -ForegroundColor Red
exit 1
}

Write-Host "Opening report..."

Start-Process .\coveragereport\index.html
