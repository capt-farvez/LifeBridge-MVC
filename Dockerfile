FROM mcr.microsoft.com/dotnet/aspnet:9.0.x AS base
WORKDIR /app
EXPOSE 80

# --- Build image ---
FROM mcr.microsoft.com/dotnet/sdk:9.0.x AS build
WORKDIR /src

# Copy and restore
COPY . .
RUN dotnet restore "LifeBridge/LifeBridge.csproj"

# Publish app
RUN dotnet publish "LifeBridge/LifeBridge.csproj" -c Release -o /app/publish

# --- Final runtime image ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0.x AS final
WORKDIR /app

# Install EF Tools for runtime
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy the app
COPY --from=build /app/publish .

# Startup: Run EF migration + start app
CMD ["sh", "-c", "dotnet ef database update && dotnet LifeBridge.dll"]