#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 9000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TomiSoft.YouTubeDownloader.WebUI/TomiSoft.YouTubeDownloader.WebUI.csproj", "TomiSoft.YouTubeDownloader.WebUI/"]
COPY ["TomiSoft.YoutubeDownloader.BusinessLogic/TomiSoft.YoutubeDownloader.BusinessLogic.csproj", "TomiSoft.YoutubeDownloader.BusinessLogic/"]
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
RUN apt-get install curl ffmpeg python -y
RUN mkdir /app/config
RUN mkdir /app/youtube-dl
RUN curl -L https://yt-dl.org/downloads/latest/youtube-dl -o /app/youtube-dl/youtube-dl
RUN chmod a+rx /app/youtube-dl/youtube-dl
ENTRYPOINT ["dotnet", "TomiSoft.YouTubeDownloader.WebUI.dll"]