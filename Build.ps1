$ErrorActionPreference = "Stop"

Write-Host "> dotnet publish postwoman" -ForegroundColor Cyan
dotnet publish .\src\Postwoman\Postwoman.csproj -c Release

Write-Host "> dotnet build installer" -ForegroundColor Cyan
dotnet build .\src\Postwoman.Installer\Postwoman.Installer.wixproj -c Release