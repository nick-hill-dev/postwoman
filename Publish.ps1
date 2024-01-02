$ErrorActionPreference = "Stop"

$baseUrl = "https://home.nick-hill.com/api-entities"
$environment = "hill-software"

$credential = Get-Credential
$authHeader = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $credential.UserName, $credential.GetNetworkCredential().Password)))

$headers = @{
    environment = $environment
    Authorization = ("Basic {0}" -f $authHeader)
}

$platforms = Invoke-RestMethod -Method Get -Headers $headers -Uri "$baseUrl/data/Platform?condition=Name eq Windows&limit=1"
$platformId = $platforms[0].id

$products = Invoke-RestMethod -Method Get -Headers $headers -Uri "$baseUrl/data/Product?condition=GroupId.Title eq Tools and Name eq Postwoman&limit=1"
$productId = $products[0].id

$version = "0.1.0"
$release = @{
    productId = $productId
    version = "0.1"
    maturity = "beta"
    releaseDate = (Get-Date).ToString("yyyy-MM-dd")
    releaseFiles = @(
        @{
            platformId = $platformId
            title = "Postwoman Installer (Windows)"
            description = "The .msi installer for the Postwoman tool."
            file = "name=postwoman.$version.msi"
        }
    )
}

$response = Invoke-RestMethod -Method Post -Headers $headers -Uri "$baseUrl/data/Release" -Body (ConvertTo-Json $release) -ContentType 'application/json'

$fileId = $response[0].files.id
Invoke-RestMethod -Method Post -Headers $headers -Uri "$baseUrl/files/$fileId" -InFile "src/Postwoman.Installer/bin/Release/Postwoman.Installer.msi"