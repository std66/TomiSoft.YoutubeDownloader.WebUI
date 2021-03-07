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
if ($lastexitcode -ne 0) {
	Write-Error "Build failed with exit code $lastexitcode"
	exit 1
}

Write-Host "Stage: Run unit tests"
dotnet test --no-build --no-restore --logger trx
if ($lastexitcode -ne 0) {
	Write-Error "Unit test execution completed with errors. Exit code: $lastexitcode"
	exit 1
}

Write-Host "Stage: Upload unit test results to AppVeyor"
if ($Env:APPVEYOR -ne $null) {
	$webclient = New-Object 'System.Net.WebClient'
	
	foreach ($TestLogFile in Get-ChildItem *.trx -Recurse) {
		$filePath = $TestLogFile.FullName
		$webclient.UploadFile("https://ci.appveyor.com/api/testresults/mstest/${env:APPVEYOR_JOB_ID}", $filePath)
		Remove-Item $filePath
	}
}

Write-Host "=== Pre-build completed ==="

Pop-Location