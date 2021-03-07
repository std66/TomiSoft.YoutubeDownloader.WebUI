docker version

$Version = "1.0.0"
$GitRepoRoot = (Resolve-Path "..\..").Path
$GitBranch = "master"

if ($Env:APPVEYOR -ne $null) {
    $Version = $Env:APPVEYOR_BUILD_VERSION
    $GitRepoRoot = $Env:APPVEYOR_BUILD_FOLDER
    $GitBranch = $Env:APPVEYOR_REPO_BRANCH
}

$BuildRoot = '.\ContinousIntegration\Build'

$ModulesToLoad = @(
    "$GitRepoRoot\$BuildRoot\Modules\Replace-AppVersion.ps1"
)

Push-Location
Set-Location $GitRepoRoot

foreach ($Module in $ModulesToLoad) {
    Write-Host "Importing module: $Module"
    Import-Module $Module
}

Replace-AppVersion -GitRepoRoot $GitRepoRoot -VersionStr $Version -GitBranch $GitBranch

Pop-Location