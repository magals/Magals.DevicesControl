using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApplicationGenMigrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "settingsdevices");

            migrationBuilder.CreateTable(
                name: "SettingsDevicesEntities",
                schema: "settingsdevices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    ConfigEntityId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsDevicesEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigEntities",
                schema: "settingsdevices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    TypeConnect = table.Column<string>(type: "text", nullable: false),
                    Protocol = table.Column<string>(type: "text", nullable: false),
                    Autoscan = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CustomsettingsEntityId = table.Column<long>(type: "bigint", nullable: false),
                    SettingsDevicesEntityId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigEntities_SettingsDevicesEntities_SettingsDevicesEntit~",
                        column: x => x.SettingsDevicesEntityId,
                        principalSchema: "settingsdevices",
                        principalTable: "SettingsDevicesEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomsettingsEntities",
                schema: "settingsdevices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ConfigEntityId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomsettingsEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomsettingsEntities_ConfigEntities_ConfigEntityId",
                        column: x => x.ConfigEntityId,
                        principalSchema: "settingsdevices",
                        principalTable: "ConfigEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigEntities_SettingsDevicesEntityId",
                schema: "settingsdevices",
                table: "ConfigEntities",
                column: "SettingsDevicesEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomsettingsEntities_ConfigEntityId",
                schema: "settingsdevices",
                table: "CustomsettingsEntities",
                column: "ConfigEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomsettingsEntities",
                schema: "settingsdevices");

            migrationBuilder.DropTable(
                name: "ConfigEntities",
                schema: "settingsdevices");

            migrationBuilder.DropTable(
                name: "SettingsDevicesEntities",
                schema: "settingsdevices");
        }
    }
}
