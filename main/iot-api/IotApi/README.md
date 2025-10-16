cd d:\Project\AI\IoT-server\main\iot-api\IotApi && dotnet ef migrations add InitialCreate
cd /d D:\Project\AI\IoT-server\main\iot-api\IotApi && dotnet ef database update

# IoT API (.NET Version)

This is the .NET version of the IoT server API, migrated from the original Java Spring Boot application. It uses PostgreSQL as the database and follows the same schema as the original application.

## Features

- User management
- Device management
- Agent configuration
- Model configuration (ASR, TTS, LLM, etc.)
- Chat history tracking
- Voiceprint recognition
- System parameters management
- Dictionary management
- **Automatic data seeding** - Initial data is automatically inserted on first run

## Technology Stack

- .NET 9.0
- PostgreSQL
- Entity Framework Core
- Swagger/OpenAPI

## Database Schema

The database schema is identical to the original Java application, with the following main tables:

- `sys_user` - System users
- `sys_user_token` - User authentication tokens
- `sys_params` - System parameters
- `sys_dict_type` - Dictionary types
- `sys_dict_data` - Dictionary data
- `ai_model_provider` - AI model providers
- `ai_model_config` - AI model configurations
- `ai_tts_voice` - TTS voices
- `ai_agent_template` - Agent templates
- `ai_agent` - Agent configurations
- `ai_device` - Device information
- `ai_voiceprint` - Voiceprint data
- `ai_chat_history` - Chat history
- `ai_chat_message` - Individual chat messages

## Getting Started

1. Update the connection string in `appsettings.json` with your PostgreSQL database details
2. Run the application: `dotnet run`
3. Access the API documentation at `http://localhost:8002/swagger`

## API Endpoints

- `/xiaozhi/agent` - Agent management
- `/xiaozhi/device` - Device management
- `/xiaozhi/models` - Model configuration
- `/xiaozhi/admin/params` - System parameters
- `/xiaozhi/user` - User authentication
- `/api/weatherforecast` - Sample weather forecast endpoint

## Data Seeding

The application automatically seeds the database with initial data on first run:

1. **Model Providers** - All supported AI model providers (ASR, TTS, LLM, VAD, Memory, Intent)
2. **Agent Templates** - Predefined agent templates for different use cases

This ensures the application has the necessary baseline data to function properly.

## Migration Status

This project is a migration of the original Java Spring Boot manager-api to .NET with PostgreSQL. The basic structure has been set up with key controllers implemented. See [MIGRATION_SUMMARY.md](MIGRATION_SUMMARY.md) for details on what has been implemented and what remains to be done.

## Running the Application

```bash
cd IotApi
dotnet run
```

The application will start on `http://localhost:8002` and Swagger documentation will be available at `http://localhost:8002/swagger`.

## Database Migrations

Entity Framework Core migrations have been created to set up the database schema. To update the database:

```bash
dotnet ef database update
```

## Health Check

A health check endpoint is available at `/health` to verify the application and database connectivity.