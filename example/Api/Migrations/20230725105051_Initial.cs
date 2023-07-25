using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebhookClients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    WebhookUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ActionType = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityTypeName = table.Column<string>(type: "TEXT", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookClients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebhookClientActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LogType = table.Column<int>(type: "INTEGER", nullable: false),
                    Log = table.Column<string>(type: "TEXT", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WebhookClientId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookClientActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebhookClientActivityLogs_WebhookClients_WebhookClientId",
                        column: x => x.WebhookClientId,
                        principalTable: "WebhookClients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WebhookClientHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    WebhookClientId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookClientHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebhookClientHeaders_WebhookClients_WebhookClientId",
                        column: x => x.WebhookClientId,
                        principalTable: "WebhookClients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebhookClientActivityLogs_WebhookClientId",
                table: "WebhookClientActivityLogs",
                column: "WebhookClientId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookClientHeaders_WebhookClientId",
                table: "WebhookClientHeaders",
                column: "WebhookClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebhookClientActivityLogs");

            migrationBuilder.DropTable(
                name: "WebhookClientHeaders");

            migrationBuilder.DropTable(
                name: "WebhookClients");
        }
    }
}
