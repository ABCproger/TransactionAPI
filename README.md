# Transaction API

## Running Docker

```bash
docker-compose up --build
```
## Navigation

- `transactionAPI`: Rest API

## Git

### Branches

- `main`: latest development version

## Installation

For backend setup 

## Requirements
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)

## Installation
1. Clone this repository to your local machine.
2. Ensure you have the required dependencies installed (listed in the Requirements section).
3. Open the solution in Visual Studio or your preferred IDE.

### NuGet Packages
Ensure the following NuGet packages are updated to the specified versions:
- `CsvHelper` v33.0.1
- `Dapper` v2.1.35
- `EPPlus` v7.2.2
- `GeoTimeZone` v5.3.0
- `Microsoft.AspNetCore.Mvc.NewtonsoftJson` v8.0.7
- `Microsoft.Data.SqlClient` v5.2.1
- `Microsoft.EntityFrameworkCore` v8.0.7
- `Microsoft.EntityFrameworkCore.Design` v8.0.7
- `Microsoft.EntityFrameworkCore.Tools` v8.0.7
- `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` v1.19.6
- `Newtonsoft.Json` v13.0.3
- `NodaTime` v3.1.11
- `Npgsql.EntityFrameworkCore.PostgreSQL` v8.0.4
- `Npgsql.EntityFrameworkCore.PostgreSQL.NodaTime` v8.0.4
- `Swashbuckle.AspNetCore` v6.4.0

## Configuration for Local Setup
*If you run the application with Docker Compose, these actions are not necessary.*

1. Configure your database connection string in `appsettings.json`.
2. Run database migrations using the Package Manager Console:
   ```shell
   Update-Database
