
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["server/server.csproj", "server/"]
COPY ["AwsS3/AwsS3.csproj", "AwsS3/"]

RUN dotnet restore "server/server.csproj"

COPY . .

WORKDIR /src/server
RUN dotnet publish "server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "server.dll"]
