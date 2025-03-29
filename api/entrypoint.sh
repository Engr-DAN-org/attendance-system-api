#!/bin/sh
export PORT=${PORT:-80}
export ASPNETCORE_HTTP_PORTS=$PORT
export ASPNETCORE_URLS=http://+:$PORT
export DOTNET_ENVIRONMENT=Production
set -e

echo "Port is set to $PORT"
echo "Waiting for database to be ready..."
attempt=0
max_attempts=20

while true; do
    echo "Attempting database migration (Attempt $attempt/$max_attempts)..."
    if dotnet-ef database update; then
        echo "Database migration successful."
        break
    else
        attempt=$((attempt + 1))
        if [ "$attempt" -ge "$max_attempts" ]; then
            echo "Reached maximum retry attempts ($max_attempts). Exiting."
            exit 1
        fi
        echo "Database migration failed. Retrying in 2 seconds..."
        sleep 2
    fi
done

echo "Starting the application..."
exec dotnet api.dll