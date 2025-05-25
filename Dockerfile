FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src

# Install EF Tools
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy and restore
COPY . .
RUN dotnet restore "LifeBridge/LifeBridge.csproj"

# Apply EF Migrations
WORKDIR /src/LifeBridge
RUN dotnet ef database update

# Publish app
RUN dotnet publish "LifeBridge.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LifeBridge.dll"]