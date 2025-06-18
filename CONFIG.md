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

### JWT Authentication Configuration

Configure JWT settings for secure token-based authentication:

```json
{
    "JwtSettings": {
        "SecretKey": "your-super-secret-key-that-is-at-least-32-characters-long",
        "Issuer": "YourAppName",
        "Audience": "YourAppUsers",
        "ExpirationInMinutes": 60
    }
}
```

**Important JWT Security Notes:**

-   **SecretKey**: Must be at least 32 characters long for security
-   **Use different keys** for Development and Production environments
-   **Never commit actual secret keys** to version control
-   **Production keys** should be randomly generated and stored securely
-   **ExpirationInMinutes**: Consider shorter times (15-60 min) for production

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

## JWT Authentication Endpoints

The application includes JWT-based authentication with the following endpoints:

### Authentication Endpoints

-   **POST** `/api/auth/login` - User login with email/password
-   **POST** `/api/auth/register` - User registration
-   **GET** `/api/auth/profile` - Get user profile (requires JWT token)
-   **POST** `/api/auth/refresh` - Refresh JWT token (requires JWT token)

### Using JWT Tokens

1. **Login/Register** to receive a JWT token
2. **Include the token** in the Authorization header for protected endpoints:
    ```
    Authorization: Bearer <your-jwt-token>
    ```
3. **Token expires** based on `ExpirationInMinutes` setting
4. **Refresh token** before expiration to maintain authentication

### Example API Calls

**Login:**

```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "user@example.com", "password": "password123"}'
```

**Access Protected Endpoint:**

```bash
curl -X GET http://localhost:5000/api/auth/profile \
  -H "Authorization: Bearer <your-jwt-token>"
```
