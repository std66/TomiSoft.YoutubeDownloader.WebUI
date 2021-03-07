Write-Host "=== Build ==="

Push-Location
Set-Location $Env:APPVEYOR_BUILD_FOLDER

$version = $Env:APPVEYOR_BUILD_VERSION
$image = "std66/tomisoft-youtubedownloader-webui"

$Env:DOCKERHUB_ACCESS_TOKEN | docker login --username std66 --password-stdin registry.hub.docker.com

docker build -t "${image}:latest" -t "${image}:${version}" .
docker push "${image}:latest"
docker push "${image}:${version}"

docker logout

Write-Host "=== Build completed ==="

Pop-Location