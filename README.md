# Battleship Royale Backend (battleship-royale-be)

## Installation Guide

Note: before following the guide below, make sure you have installed the following:
- .NET
- Docker

Follow these steps to set up the project:

1. Download the project to your local machine by cloning it.
2. Navigate to the root of the project and run the following command to create docker container with mssql database: `./start-db.sh`
3. Navigate to project's root folder and run the following command to build a project: `dotnet build`
5. After the build process is finished, execute the following command to run the project: `dotnet run`
6. In the console, you will see information about the ports on which the project is running. 
