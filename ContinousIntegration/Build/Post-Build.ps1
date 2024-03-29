$ErrorActionPreference = "Stop";

Write-Host "=== Post build ==="

Write-Host "Stage: Publish Docker image"

$version = $Env:APPVEYOR_BUILD_VERSION
$image = "std66/tomisoft-youtubedownloader-webui"

docker logout
$Env:DOCKERHUB_ACCESS_TOKEN | docker login --username std66 --password-stdin

docker push "${image}"
docker push "${image}:${version}"

docker logout

Write-Host "Stage: Tag commit in git"

Push-Location
Set-Location $Env:APPVEYOR_BUILD_FOLDER

git config --global credential.helper store
git config --global user.email "sinkutamas@gmail.com"
git config --global user.name "AppVeyor"
Add-Content -Path "$HOME\.git-credentials" -Value "https://oauth2:$($env:GIT_ACCESS_TOKEN)@github.com`n" -NoNewline

$version = $Env:APPVEYOR_BUILD_VERSION
$tagname = "build_$version"

Write-Host "Git tag: $tagname"

git tag $tagname
git push origin $tagname

Write-Host "=== Post build completed ==="

Pop-Location