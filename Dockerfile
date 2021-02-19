#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# soft link
RUN ln -s /lib/x86_64-linux-gnu/libdl-2.24.so /lib/x86_64-linux-gnu/libdl.so

# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
   		libgdiplus \
#         libc6-dev \
#         libgdiplus \
#         libx11-dev \
     && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Server/AlfalahApp.Server.csproj", "Server/"]
COPY ["Shared/AlfalahApp.Shared.csproj", "Shared/"]
COPY ["Client/AlfalahApp.Client.csproj", "Client/"]
RUN dotnet restore "Server/AlfalahApp.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "AlfalahApp.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AlfalahApp.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet AlfalahApp.Server.dll