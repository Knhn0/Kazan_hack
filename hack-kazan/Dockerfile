FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["hack-kazan/hack-kazan.csproj", "hack-kazan/"]
RUN dotnet restore "hack-kazan/hack-kazan.csproj"
COPY . .
WORKDIR "/src/hack-kazan"
RUN dotnet build "hack-kazan.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "hack-kazan.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "hack-kazan.dll"]
