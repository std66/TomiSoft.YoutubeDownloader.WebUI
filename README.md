# TomiSoft.YoutubeDownloader.WebUI
[![Build status](https://ci.appveyor.com/api/projects/status/uxm5u7fr5752mr92?svg=true)](https://ci.appveyor.com/project/std66/tomisoft-youtubedownloader-webui)
[![BCH compliance](https://bettercodehub.com/edge/badge/std66/TomiSoft.YoutubeDownloader.WebUI?branch=master)](https://bettercodehub.com/)

A simple WebUI for youtube-dl.

Requirements
------------
  - Microsoft Visual Studio 2019
  - Microsoft .NET 6.0
  - IIS 8.0 or newer with WebSockets enabled

Run in Docker
-------------
The easiest way to get started is to use my Docker image. It is preconfigured and it is immediately ready to use.
https://hub.docker.com/r/std66/tomisoft-youtubedownloader-webui

```
docker run -d -p 28465:80 -p 30000:9000 std66/tomisoft-youtubedownloader-webui
```
Open http://localhost:28465 for the application. Prometheus metrics are available at http://localhost:30000/metrics

Running in Kubernetes
---------------------
It will be available in the future. Since the application is heavily stateful, it needs some more refactoring work to be done to fully utilitize the benefits of Kubernetes.

Features available at the moment:
- Can run in Kubernetes with 1 pod
- Can be configured using Kubernetes ConfigMap

Manual deploy to IIS
--------------------
  * Download the executable youtube-dl from https://rg3.github.io/youtube-dl/
  * Download ffmpeg, ffprobe from https://www.ffmpeg.org/download.html and extract them next to the youtube-dl executable.
  * Add IUSR read+execute rights to the folder that contains youtube-dl.
  * Build solution and deploy TomiSoft.YoutubeDownloader.WebUI project to IIS:
    * Create a new IIS website
    * Configure the website's application pool:
      * Set .NET CLR Version to "No managed code".
      * The process model identity must be set to "LocalSystem", otherwise the youtube-dl cannot be executed.
    * Deploy website
    * Configure appsettings.json
      * Set ExecutablePath to the path where the youtube-dl.exe is located.
      * Set MaximumParallelDownloads according to how many downloads can be running simultaneously.
      * Set DeleteFilesAfterMinutesElapsed according to how long do you want to keep downloaded files.
  * Execute IISRESET in cmd.exe

Screenshots
-----------
  ![Screenshot 1](https://i.postimg.cc/8577MQnb/K-pkiv-g-s.png)
  ![Screenshot 2](https://i.postimg.cc/rpndDFgm/K-pkiv-g-s-2.png)

Contact
-------
  Tamás Sinku (sinkutamas@gmail.com)
