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
                    Enable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsDevicesEntities", x => x.Name);
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
                    Type_Connect = table.Column<string>(type: "text", nullable: false),
                    Config = table.Column<string>(type: "text", nullable: false),
                    Protocol = table.Column<string>(type: "text", nullable: false),
                    Autoscan = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    SettingsDevicesEntityName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigEntities", x => x.Name);
                    table.ForeignKey(
                        name: "FK_ConfigEntities_SettingsDevicesEntities_SettingsDevicesEntit~",
                        column: x => x.SettingsDevicesEntityName,
                        principalSchema: "settingsdevices",
                        principalTable: "SettingsDevicesEntities",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
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
                    ConfigsName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomsettingsEntities", x => x.Key);
                    table.ForeignKey(
                        name: "FK_CustomsettingsEntities_ConfigEntities_ConfigsName",
                        column: x => x.ConfigsName,
                        principalSchema: "settingsdevices",
                        principalTable: "ConfigEntities",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigEntities_SettingsDevicesEntityName",
                schema: "settingsdevices",
                table: "ConfigEntities",
                column: "SettingsDevicesEntityName");

            migrationBuilder.CreateIndex(
                name: "IX_CustomsettingsEntities_ConfigsName",
                schema: "settingsdevices",
                table: "CustomsettingsEntities",
                column: "ConfigsName");
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
