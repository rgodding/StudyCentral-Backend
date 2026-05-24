@echo off

if not exist Azurite (
    mkdir Azurite
)

start cmd /k "azurite --location ./Azurite --skipApiVersionCheck"

dotnet run