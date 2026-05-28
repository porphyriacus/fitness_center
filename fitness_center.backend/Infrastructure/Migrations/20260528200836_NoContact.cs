using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NoContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact_Type",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Contact_Value",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Contact_Type",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Contact_Value",
                table: "Clients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contact_Type",
                table: "Trainers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Contact_Value",
                table: "Trainers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Contact_Type",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Contact_Value",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
