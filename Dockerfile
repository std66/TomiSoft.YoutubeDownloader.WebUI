#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["TomiSoft.YouTubeDownloader.WebUI/TomiSoft.YouTubeDownloader.WebUI.csproj", "TomiSoft.YouTubeDownloader.WebUI/"]
COPY ["TomiSoft.Common/TomiSoft.Common.csproj", "TomiSoft.Common/"]
COPY ["Tomisoft.YoutubeDownloader/TomiSoft.YoutubeDownloader.csproj", "Tomisoft.YoutubeDownloader/"]
RUN dotnet restore "TomiSoft.YouTubeDownloader.WebUI/TomiSoft.YouTubeDownloader.WebUI.csproj"
COPY . .
#set NLog config for Linux
RUN rm "TomiSoft.YouTubeDownloader.WebUI/nlog.config"
RUN mv "TomiSoft.YouTubeDownloader.WebUI/nlog-linux.config" "TomiSoft.YouTubeDownloader.WebUI/nlog.config"
#set appsettings.json for Linux
RUN rm "TomiSoft.YouTubeDownloader.WebUI/appsettings.json"
RUN mv "TomiSoft.YouTubeDownloader.WebUI/appsettings-linux.json" "TomiSoft.YouTubeDownloader.WebUI/appsettings.json"

WORKDIR "/src/TomiSoft.YouTubeDownloader.WebUI"
RUN dotnet build "TomiSoft.YouTubeDownloader.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TomiSoft.YouTubeDownloader.WebUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update
RUN apt-get install ffmpeg python -y
RUN mkdir /app/youtube-dl
RUN curl -L https://yt-dl.org/downloads/latest/youtube-dl -o /app/youtube-dl/youtube-dl
RUN chmod a+rx /app/youtube-dl/youtube-dl
ENTRYPOINT ["dotnet", "TomiSoft.YouTubeDownloader.WebUI.dll"]