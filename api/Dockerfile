# Base image with SDK (used for both dev & build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /app
EXPOSE 5182

# Set build argument (default to Development)
ARG BUILD_ENV=Development
ENV ASPNETCORE_ENVIRONMENT=$BUILD_ENV

# ===========================
# DEVELOPMENT STAGE (Hot Reload)
# ===========================
FROM base AS dev

# Enable polling file watcher for hot reload
ENV DOTNET_USE_POLLING_FILE_WATCHER=1

# Install EF Core CLI tools
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy project file & restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the files
COPY . .

# Run with the migration files
RUN dotnet-ef database update

# Run with hot reload and automatic restart
CMD ["dotnet", "watch", "run", "--no-hot-restart", "--accept-hot-reload", "--urls", "http://0.0.0.0:5182"]

# ===========================
# PRODUCTION STAGE (Optimized Build)
# ===========================
FROM base AS build

# Override environment for production
ARG BUILD_ENV=Production
ENV ASPNETCORE_ENVIRONMENT=$BUILD_ENV

# Copy project file & restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy all files & publish optimized build
COPY . .
RUN dotnet publish -c Release -o /app/out

# ===========================
# RUNTIME STAGE (Lightweight)
# ===========================
# FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
FROM base AS runtime
WORKDIR /app
EXPOSE 8080

# Copy built app from build stage
COPY --from=build /app/out .

# Copy the project file (needed for dotnet-ef)
COPY --from=build /app/*.csproj . 

# Copy the Migrations folder (if not included in publish output)
# COPY --from=build /app/Migrations ./Migrations

# Install EF Core CLI tools
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

ENV ASPNETCORE_HTTP_PORTS=${PORT:-80}
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
# Expose port

# Copy the entrypoint script
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh


# Use the script as the container entrypoint
ENTRYPOINT ["/app/entrypoint.sh"]
