#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN echo $(ls)

RUN dotnet restore SmartMirrorHubV6.Website/SmartMirrorHubV6.Website.csproj
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish SmartMirrorHubV6.Website/SmartMirrorHubV6.Website.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV TZ=Pacific/Auckland
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
ENTRYPOINT ["dotnet", "SmartMirrorHubV6.Website.dll"]