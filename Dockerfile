#===BUILD===
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY ["TomiSoft.YouTubeDownloader.WebUI/TomiSoft.YouTubeDownloader.WebUI.csproj", "TomiSoft.YouTubeDownloader.WebUI/"]
COPY ["TomiSoft.YoutubeDownloader.BusinessLogic/TomiSoft.YoutubeDownloader.BusinessLogic.csproj", "TomiSoft.YoutubeDownloader.BusinessLogic/"]
COPY ["TomiSoft.Common/TomiSoft.Common.csproj", "TomiSoft.Common/"]
COPY ["Tomisoft.YoutubeDownloader/TomiSoft.YoutubeDownloader.csproj", "Tomisoft.YoutubeDownloader/"]

RUN dotnet restore "TomiSoft.YouTubeDownloader.WebUI/TomiSoft.YouTubeDownloader.WebUI.csproj"

COPY . .

RUN mv "TomiSoft.YouTubeDownloader.WebUI/nlog-linux.config" "TomiSoft.YouTubeDownloader.WebUI/nlog.config" && \
    mv "TomiSoft.YouTubeDownloader.WebUI/appsettings-linux.json" "TomiSoft.YouTubeDownloader.WebUI/appsettings.json"

WORKDIR "/src/TomiSoft.YouTubeDownloader.WebUI"
RUN dotnet publish "TomiSoft.YouTubeDownloader.WebUI.csproj" -c Release -o /app/publish

RUN mkdir -p /app/yt-temp && \
    curl -L https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp -o /app/yt-temp/youtube-dl && \
    chmod a+rx /app/yt-temp/youtube-dl

#===RUNTIME===
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=denoland/deno:bin /deno /usr/local/bin/deno

RUN apt-get update && \
    apt-get install -y ffmpeg python3 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

RUN mkdir -p /app/youtube-dl
COPY --from=build /app/yt-temp/youtube-dl /app/youtube-dl/youtube-dl

RUN mkdir -p /app/config

ENV ASPNETCORE_URLS=http://0.0.0.0:80
EXPOSE 80
EXPOSE 9000

ENTRYPOINT ["dotnet", "TomiSoft.YouTubeDownloader.WebUI.dll"]