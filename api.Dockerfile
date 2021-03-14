FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .

RUN chmod +x ./DevopsConfig/migrate.sh
RUN dotnet publish -c release -o /app
RUN ["./DevopsConfig/migrate.sh"]


#Production
#PC
FROM mcr.microsoft.com/dotnet/aspnet:5.0.4-alpine3.13-amd64
#Pi
#FROM mcr.microsoft.com/dotnet/aspnet:5.0.4-buster-slim-arm32v7

WORKDIR /app
COPY --from=base /app ./

EXPOSE 80
ENTRYPOINT ["dotnet", "Mutify.dll"]
