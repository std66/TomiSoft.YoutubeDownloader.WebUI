$Version = "1.0.0"
$GitRepoRoot = (Resolve-Path "../..").Path
$GitBranch = "master"

if ($Env:APPVEYOR -ne $null) {
    $Version = $Env:APPVEYOR_BUILD_VERSION
    $GitRepoRoot = $Env:APPVEYOR_BUILD_FOLDER
    $GitBranch = $Env:APPVEYOR_REPO_BRANCH
}

$BuildRoot = './ContinousIntegration/Build'

$ModulesToLoad = @(
    "$GitRepoRoot/$BuildRoot/Modules/Replace-AppVersion.ps1"
)

Push-Location
Set-Location $GitRepoRoot

foreach ($Module in $ModulesToLoad) {
    Write-Host "Importing module: $Module"
    Import-Module $Module
}

Write-Host "=== Pre-build ==="

Write-Host "Stage: Print Docker information"
docker version

Write-Host "Stage: Configuring application version info"
Replace-AppVersion -GitRepoRoot $GitRepoRoot -VersionStr $Version -GitBranch $GitBranch

Write-Host "Stage: Build"
dotnet build

Write-Host "Stage: Run unit tests"
dotnet test

Write-Host "=== Pre-build completed ==="

Pop-Location