# Configuration Setup

This project uses different configuration methods for different environments and deployment scenarios.

## ASP.NET Core Configuration (appsettings.json)

For the main application configuration:

### Setup Instructions

1. Copy the example configuration files:

    ```bash
    cp appsettings.example.json appsettings.json
    cp appsettings.Development.example.json appsettings.Development.json
    ```

2. Update the connection strings in both files with your actual database credentials:

    - `appsettings.json` - Production/default configuration
    - `appsettings.Development.json` - Development configuration

3. The actual `appsettings.json` and `appsettings.Development.json` files are ignored by git to keep secrets secure.

### Database Configuration

Update the `DefaultConnection` connection string with your PostgreSQL credentials:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Database=absence_tracker;Username=your_username;Password=your_password"
    }
}
```

### Environment Variables

The application uses the `ASPNETCORE_ENVIRONMENT` variable to determine which configuration to load:

-   `Development` - Uses `appsettings.Development.json`
-   `Production` - Uses `appsettings.json`

## Docker Configuration (.env)

For Docker Compose deployment, this project uses a `.env` file to configure the containerized PostgreSQL database and pgAdmin.

### Setup Instructions

1. Copy the example environment file:

    ```bash
    cp .env.example .env
    ```

2. Update the `.env` file with your actual configuration:

    - Replace all placeholder values with your secure credentials
    - Choose strong passwords for database and pgAdmin access

3. **Never commit the `.env` file to version control** - it contains sensitive information

### Environment Variables

The `.env` file is used by `docker-compose.yml` and contains:

-   **POSTGRES_USER** - PostgreSQL database username
-   **POSTGRES_PASSWORD** - PostgreSQL database password
-   **POSTGRES_DB** - Database name to create
-   **PGADMIN_DEFAULT_EMAIL** - Email for pgAdmin web interface login
-   **PGADMIN_DEFAULT_PASSWORD** - Password for pgAdmin web interface
-   **PGADMIN_PORT** - Port for pgAdmin web interface (default: 5050)
