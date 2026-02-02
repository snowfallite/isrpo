FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Client.csproj", "./"]
RUN dotnet restore "Client.csproj"
COPY . .
RUN dotnet publish "Client.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Client.dll"]