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
   git clone https://github.com/yourusername/PasswordManagerAPI.git
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




