using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureTaskTeamApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelsWithRolesAndDeadlines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Tasks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Tasks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "deadLine",
                table: "Tasks",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "deadLine",
                table: "Tasks");
        }
    }
}
