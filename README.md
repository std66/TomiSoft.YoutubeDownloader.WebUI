# TomiSoft.YoutubeDownloader.WebUI
[![Build status](https://ci.appveyor.com/api/projects/status/uxm5u7fr5752mr92?svg=true)](https://ci.appveyor.com/project/std66/tomisoft-youtubedownloader-webui)

A simple WebUI for youtube-dl.

Requirements
------------
  - Microsoft Visual Studio 2017
  - Microsoft .NET Core 2.1

Get started
-----------
  * Download the executable youtube-dl from https://rg3.github.io/youtube-dl/
  * Configure appsettings.json
    * Set ExecutablePath to the path where the youtube-dl.exe is located.
    * Set MaximumParallelDownloads according to how many downloads can be running simultaneously.
    * Set DeleteFilesAfterMinutesElapsed according to how long do you want to keep downloaded files.
  * Build solution and deploy TomiSoft.YoutubeDownloader.WebUI project to IIS.
  
Contact
-------
  Sinku Tamás (sinkutamas@gmail.com)
