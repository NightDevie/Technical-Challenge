
# Technical-Challenge

## Overview
This project was built as a technical challenge to create an authentication API in .NET 8. It validates user credentials stored in a PostgreSQL database using BCrypt and issues JWT access and refresh tokens. The API follows the OAuth 2.0 password grant standard and is designed to replace a legacy Java authentication system seamlessly.

The Challenge and files needed are in the following repository:
https://bitbucket.org/letshare-dev/dev-2025/src/master/

## Running PostgreSQL with Docker and Importing the Database
Both `docker-compose.yml` and `init_database.sql` are located in the root folder of this project.

Using Windows, open the PowerShell terminal inside Docker or another compatible terminal like the Linux terminal and use the following commands:

First:
```powershell
cd "replace with your full project root location"
```

Then:
```bash
docker-compose up
```

## Executing the project

In the project root folder use the following commands:

```bash
dotnet build
dotnet run
```

And now you're free to run the tests using Postman with the following POST request:

```
http://localhost:5000/api/auth/login
```

Payload:
```json
{
  "grantType": "password",
  "clientId": "web",
  "clientSecret": "webpass1",
  "username": "assessment@letshare-test.com",
  "password": "devtest2025"
}
```
