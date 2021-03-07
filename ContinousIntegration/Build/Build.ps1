Write-Host "=== Build ==="

Push-Location
Set-Location $Env:APPVEYOR_BUILD_FOLDER

$version = $Env:APPVEYOR_BUILD_VERSION
$image = "std66/tomisoft-youtubedownloader-webui"

docker logout
$Env:DOCKERHUB_ACCESS_TOKEN | docker login --username std66 --password-stdin

Write-Host "Stage: Build Docker image"

#docker build -t "local_image" .
#
#docker tag "local_image:latest" "${image}"
#docker tag "local_image:latest" "${image}:${version}"

docker buildx create --name mybuilder --use
docker buildx build --platform "linux/amd64,linux/arm64" -t "${image}" -t "${image}:${version}" --push .

Write-Host "Stage: Publish Docker image"

#docker push "${image}"
#docker push "${image}:${version}"

docker logout

Write-Host "=== Build completed ==="

Pop-Location