$BuildRoot = './ContinousIntegration/Build'

function Replace-AppVersion([string] $GitRepoRoot, [string] $VersionStr, [string] $GitBranch) {
    Push-Location
    Set-Location $GitRepoRoot

    Write-Host "Replacing AppVersion.cs..."

    $File = "$GitRepoRoot/TomiSoft.YouTubeDownloader.WebUI/AppVersion.cs"
    $Contents = Get-ContentsOfAppVersionClass -GitRepoRoot $GitRepoRoot -VersionStr $VersionStr -GitBranch $GitBranch

    Remove-Item $File
    $Contents | Out-File $File -Encoding utf8

    Write-Host "Successfully replaced AppVersion.cs"

    Pop-Location
}

function Get-ContentsOfAppVersionClass([string] $GitRepoRoot, [string] $VersionStr, [string] $GitBranch) {
    $CurrentDateStr = [System.DateTimeOffset]::UtcNow.ToString('o')
    $ShortCommitHash = $(git rev-parse --short HEAD)
    $CommitHash = $(git rev-parse HEAD)

    $Template = Get-Content "$GitRepoRoot/$BuildRoot/Templates/AppVersion.cs"

    $Template = $Template.Replace("GIT_COMMIT_HASH", $CommitHash)
    $Template = $Template.Replace("GIT_SHORT_COMMIT_HASH", $ShortCommitHash)
    $Template = $Template.Replace("GIT_BRANCH", $GitBranch)
    $Template = $Template.Replace("BUILD_TIME", $CurrentDateStr)
    $Template = $Template.Replace("VERSION", $VersionStr)

    return $Template
}