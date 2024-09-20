#!/bin/bash

# Variables
CONTAINER_NAME="mssql-container"
SA_PASSWORD="YourStrong!Passw0rd"  # Replace with your strong password
MSSQL_IMAGE="mcr.microsoft.com/mssql/server:latest"
ACCEPT_EULA="Y"
PORT=1433  # Default MSSQL port

# Function to check if Docker is installed
check_docker_installed() {
    if ! [ -x "$(command -v docker)" ]; then
        echo "Error: Docker is not installed." >&2
        exit 1
    fi
}

# Function to check if the container is already running
check_container_running() {
    if [ "$(docker ps -q -f name=$CONTAINER_NAME)" ]; then
        echo "Container $CONTAINER_NAME is already running."
        exit 0
    fi
}

# Function to start MSSQL Docker container
start_mssql_container() {
    echo "Pulling the latest MSSQL Server Docker image..."
    docker pull $MSSQL_IMAGE

    echo "Running the MSSQL container..."
    docker run -e 'ACCEPT_EULA=Y' \
               -e "SA_PASSWORD=$SA_PASSWORD" \
               -p $PORT:1433 \
               --name $CONTAINER_NAME \
               -d $MSSQL_IMAGE

    echo "MSSQL Server is now running in container: $CONTAINER_NAME"
}

# Main script
check_docker_installed
check_container_running
start_mssql_container

# Display container status
echo "Container status:"
docker ps -f name=$CONTAINER_NAME