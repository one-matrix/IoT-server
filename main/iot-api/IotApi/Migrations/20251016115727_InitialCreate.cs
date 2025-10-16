using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IotApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ai_agent",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    agent_code = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    agent_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    asr_model_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    vad_model_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    llm_model_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    tts_model_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    tts_voice_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    mem_model_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    intent_model_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    system_prompt = table.Column<string>(type: "text", nullable: false),
                    lang_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_agent", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ai_agent_template",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    agent_code = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    agent_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    asr_model_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    vad_model_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    llm_model_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    tts_model_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    tts_voice_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    mem_model_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    intent_model_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    system_prompt = table.Column<string>(type: "text", nullable: false),
                    lang_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_agent_template", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ai_chat_history",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    agent_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    device_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    message_count = table.Column<int>(type: "integer", nullable: true),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_chat_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ai_chat_message",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    chat_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    prompt_tokens = table.Column<int>(type: "integer", nullable: true),
                    total_tokens = table.Column<int>(type: "integer", nullable: true),
                    completion_tokens = table.Column<int>(type: "integer", nullable: true),
                    prompt_ms = table.Column<int>(type: "integer", nullable: true),
                    total_ms = table.Column<int>(type: "integer", nullable: true),
                    completion_ms = table.Column<int>(type: "integer", nullable: true),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_chat_message", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ai_device",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    mac_address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_connected_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    auto_update = table.Column<int>(type: "integer", nullable: true),
                    board = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    alias = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    agent_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    app_version = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_device", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ai_model_config",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    model_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    model_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    model_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_default = table.Column<int>(type: "integer", nullable: true),
                    is_enabled = table.Column<int>(type: "integer", nullable: true),
                    config_json = table.Column<string>(type: "text", nullable: false),
                    doc_link = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    remark = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_model_config", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ai_model_provider",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    model_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    provider_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    fields = table.Column<string>(type: "text", nullable: false),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_model_provider", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ai_ota",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    firmware_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    version = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: true),
                    remark = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    firmware_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_ota", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ai_tts_voice",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    tts_model_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    tts_voice = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    languages = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    voice_demo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    remark = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_tts_voice", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ai_voice_clone",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    model_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    voice_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    voice = table.Column<byte[]>(type: "bytea", nullable: false),
                    train_status = table.Column<int>(type: "integer", nullable: true),
                    train_error = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_voice_clone", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ai_voiceprint",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    agent_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    agent_code = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    agent_name = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    embedding = table.Column<string>(type: "text", nullable: false),
                    memory = table.Column<string>(type: "text", nullable: false),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_voiceprint", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_dict_data",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    dict_type_id = table.Column<long>(type: "bigint", nullable: false),
                    dict_label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    dict_value = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    remark = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_dict_data", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_dict_type",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    dict_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    dict_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    remark = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_dict_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_params",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    param_code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    param_value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    param_type = table.Column<int>(type: "integer", nullable: true),
                    remark = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_params", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    super_admin = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updater = table.Column<long>(type: "bigint", nullable: true),
                    creator = table.Column<long>(type: "bigint", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_user_token",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    token = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    expire_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    create_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_user_token", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ai_agent");

            migrationBuilder.DropTable(
                name: "ai_agent_template");

            migrationBuilder.DropTable(
                name: "ai_chat_history");

            migrationBuilder.DropTable(
                name: "ai_chat_message");

            migrationBuilder.DropTable(
                name: "ai_device");

            migrationBuilder.DropTable(
                name: "ai_model_config");

            migrationBuilder.DropTable(
                name: "ai_model_provider");

            migrationBuilder.DropTable(
                name: "ai_ota");

            migrationBuilder.DropTable(
                name: "ai_tts_voice");

            migrationBuilder.DropTable(
                name: "ai_voice_clone");

            migrationBuilder.DropTable(
                name: "ai_voiceprint");

            migrationBuilder.DropTable(
                name: "sys_dict_data");

            migrationBuilder.DropTable(
                name: "sys_dict_type");

            migrationBuilder.DropTable(
                name: "sys_params");

            migrationBuilder.DropTable(
                name: "sys_user");

            migrationBuilder.DropTable(
                name: "sys_user_token");
        }
    }
}
