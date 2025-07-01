FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["TomiSoft.YouTubeDownloader.WebUI/TomiSoft.YouTubeDownloader.WebUI.csproj", "TomiSoft.YouTubeDownloader.WebUI/"]
COPY ["TomiSoft.YoutubeDownloader.BusinessLogic/TomiSoft.YoutubeDownloader.BusinessLogic.csproj", "TomiSoft.YoutubeDownloader.BusinessLogic/"]
COPY ["TomiSoft.Common/TomiSoft.Common.csproj", "TomiSoft.Common/"]
COPY ["Tomisoft.YoutubeDownloader/TomiSoft.YoutubeDownloader.csproj", "Tomisoft.YoutubeDownloader/"]

RUN dotnet restore "TomiSoft.YouTubeDownloader.WebUI/TomiSoft.YouTubeDownloader.WebUI.csproj"

COPY . .

RUN rm "TomiSoft.YouTubeDownloader.WebUI/nlog.config"
RUN mv "TomiSoft.YouTubeDownloader.WebUI/nlog-linux.config" "TomiSoft.YouTubeDownloader.WebUI/nlog.config"
RUN rm "TomiSoft.YouTubeDownloader.WebUI/appsettings.json"
RUN mv "TomiSoft.YouTubeDownloader.WebUI/appsettings-linux.json" "TomiSoft.YouTubeDownloader.WebUI/appsettings.json"

WORKDIR "/src/TomiSoft.YouTubeDownloader.WebUI"

RUN dotnet publish "TomiSoft.YouTubeDownloader.WebUI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish . 

RUN apt-get update && \
    apt-get install -y curl ffmpeg python3 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

RUN mkdir /app/config && \
    mkdir /app/youtube-dl && \
    curl -L https://yt-dl.org/downloads/latest/youtube-dl -o /app/youtube-dl/youtube-dl && \
    chmod a+rx /app/youtube-dl/youtube-dl

# ENV LC_ALL=en_US.UTF-8
# ENV LANG=en_US.UTF-8

EXPOSE 80
EXPOSE 9000

ENTRYPOINT ["./TomiSoft.YouTubeDownloader.WebUI"]