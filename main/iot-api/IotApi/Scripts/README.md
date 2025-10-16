# Database Initialization Scripts

This directory contains SQL scripts for initializing the PostgreSQL database with the same initial data that is seeded programmatically in the .NET application.

## Scripts

1. [initial-model-providers.sql](file://d:\Project\AI\IoT-server\main\iot-api\IotApi\Scripts\initial-model-providers.sql) - Inserts all AI model providers (VAD, ASR, LLM, TTS, Memory, Intent)
2. [initial-agent-templates.sql](file://d:\Project\AI\IoT-server\main\iot-api\IotApi\Scripts\initial-agent-templates.sql) - Inserts predefined agent templates
3. [initial-firmware-types.sql](file://d:\Project\AI\IoT-server\main\iot-api\IotApi\Scripts\initial-firmware-types.sql) - Inserts firmware type dictionary data
4. [initial-mobile-areas.sql](file://d:\Project\AI\IoT-server\main\iot-api\IotApi\Scripts\initial-mobile-areas.sql) - Inserts mobile area dictionary data
5. [initialize-database.sql](file://d:\Project\AI\IoT-server\main\iot-api\IotApi\Scripts\initialize-database.sql) - Master script that combines all initialization scripts

## Usage

These scripts can be used to manually initialize the database or as part of a database migration process:

```bash
# Run individual scripts using psql
psql -h hostname -U username -d database_name -f initial-model-providers.sql
psql -h hostname -U username -d database_name -f initial-agent-templates.sql
psql -h hostname -U username -d database_name -f initial-firmware-types.sql
psql -h hostname -U username -d database_name -f initial-mobile-areas.sql

# Or run the master script
psql -h hostname -U username -d database_name -f initialize-database.sql
```

Or execute them in your preferred PostgreSQL client tool.

## Notes

- These scripts will delete existing data in the respective tables before inserting new data
- The data is identical to what is programmatically seeded in the DataSeeder.cs class
- These scripts are provided as an alternative to the programmatic seeding for environments where that approach is not preferred
- The [initialize-database.sql](file://d:\Project\AI\IoT-server\main\iot-api\IotApi\Scripts\initialize-database.sql) script contains all initialization data in a single file