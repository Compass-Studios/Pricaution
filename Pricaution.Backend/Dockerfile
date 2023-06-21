FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Pricaution.Backend/Pricaution.Backend.csproj", "Pricaution.Backend/"]
RUN dotnet restore "Pricaution.Backend/Pricaution.Backend.csproj"
COPY . .
WORKDIR "/src/Pricaution.Backend"
RUN dotnet build "Pricaution.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pricaution.Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pricaution.Backend.dll"]
