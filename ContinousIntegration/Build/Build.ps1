Write-Host "=== Build ==="

Push-Location
Set-Location $Env:APPVEYOR_BUILD_FOLDER

$version = $Env:APPVEYOR_BUILD_VERSION
$image = "std66/tomisoft-youtubedownloader-webui"

docker logout
$Env:DOCKERHUB_ACCESS_TOKEN | docker login --username std66 --password-stdin

docker build -t "local_image" .

docker tag "local_image:latest" "${image}"
docker tag "local_image:latest" "${image}:${version}"

docker push "${image}"
docker push "${image}:${version}"

docker logout

Write-Host "=== Build completed ==="

Pop-Location