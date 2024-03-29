$ErrorActionPreference = "Stop";

Write-Host "=== Build ==="

Push-Location
Set-Location $Env:APPVEYOR_BUILD_FOLDER

$version = $Env:APPVEYOR_BUILD_VERSION
$image = "std66/tomisoft-youtubedownloader-webui"

Write-Host "Stage: Build Docker image"

docker build -t "local_image" .

docker tag "local_image:latest" "${image}"
docker tag "local_image:latest" "${image}:${version}"

Write-Host "=== Build completed ==="

Pop-Location