using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TgBot.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BotUserDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UserTelNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserNameSearch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserLast = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Api_Access_Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Api_HemisId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Api_FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Api_LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Api_Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Api_Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Api_Group = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotUserDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileTgAdressDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fileTgAdressId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileTgAdressDb", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotUserDB");

            migrationBuilder.DropTable(
                name: "FileTgAdressDb");
        }
    }
}
