# Docker Setup with SQL Server, Database Creation and Password Manager API
## Prerequisites

- Docker installed on your machine.
- SQL Server tools (optional, for using `sqlcmd` from the host).

## Step 1: Create a Dockerfile

Create a `Dockerfile` in your project directory with the following content:

```dockerfile
# Use the official SQL Server 2019 image
FROM mcr.microsoft.com/mssql/server:2019-latest

# Set the environment variables for SQL Server
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=J@gadeesh7868 --yourp assword
ENV MSSQL_PID=Developer

# Expose the default SQL Server port
EXPOSE 1433

# Run SQL Server
CMD /opt/mssql/bin/sqlservr

## Step 2: Build the Docker Image

In your terminal, navigate to the directory containing the Dockerfile and run:

`docker build -t my-sql-server .`

## step 3: Run the SQL Server Container

Start the SQL Server container using the following command:

`docker run -d -p 1433:1433 --name sql_server_container my-sql-server`

## step 4: Create Database and Table

Using sqlcmd from Host

`sqlcmd -S localhost -U SA -P 'your@password'`

# Create Database and Table

`CREATE DATABASE PasswordManagerDB;
GO`

# Use Database

`USE PasswordManagerDB;
GO`

# Create Table

CREATE TABLE Passwords (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Category NVARCHAR(255) NOT NULL,
    App NVARCHAR(255) NOT NULL,
    UserName NVARCHAR(255) NOT NULL,
    EncryptedPassword NVARCHAR(MAX) NOT NULL
);
GO

# Password Manager API

## Overview

Password Manager API is a secure web service for managing passwords. It allows users to store, retrieve, update, and delete passwords securely, using encryption for enhanced security.

## Features

- **CRUD Operations**: Create, Read, Update, and Delete password entries.
- **Password Encryption**: All passwords are stored in an encrypted format using Base64 encoding.
- **In-memory Database**: The API uses an in-memory database for easy testing and development.

## Tech Stack

- **Backend**: .NET 6 (C#)
- **Frontend**: Angular
- **Database**: SQL Server (for production), In-memory database (for testing)
- **Testing**: xUnit, Moq for unit tests

## Getting Started

### Prerequisites

Make sure you have the following installed:

- [.NET SDK 6.0 or higher](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (if you choose to use it)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/JagadeeshGiribabu/PasswordManagerApi.git
   cd PasswordManagerAPI

2. Restore dependencies:

    dotnet restore

3. Configure the database connection

    Update the appsettings.json file with your SQL Server connection string

4. Run the application:

    dotnet run

### API Endpoints

The API provides the following endpoints:

1. Get All Passwords
    URL: /api/passwords
    Method: GET
    Response: Returns a list of all passwords.

2. Get Password by ID
    URL: /api/passwords/{id}/{includeDecrypted} - optional perameter
    Method: GET
    Response: Returns a specific password by ID.

3. Add Password
    URL: /api/passwords
    Method: POST
    Request Body:
    {
        "userName": "exampleUser",
        "app": "exampleApp",
        "category": "exampleCategory",
        "encryptedPassword": "yourPassword"
    }
    Response: Returns the created password object.

4. Update Password
    URL: /api/passwords/{id}
    Method: PUT
    Request Body:
    {
        "userName": "updatedUser",
        "app": "updatedApp",
        "category": "updatedCategory",
        "encryptedPassword": "updatedPassword"
    }
    Response: Returns the updated password object.

5. Delete Password
    URL: /api/passwords/{id}
    Method: DELETE
    Response: Returns a success message if the password was deleted.

### Tests

- To run the unit tests for the Password Manager API, use the following command:
    **dotnet test**

## Test Cases

    Get All Passwords: Validates that all passwords are retrieved correctly.
    Get Password by ID: Checks retrieval of a password by its ID.
    Add Password: Confirms that a password can be added and is encrypted.
    Update Password: Ensures a password can be updated correctly.
    Delete Password: Validates that a password can be deleted successfully.




