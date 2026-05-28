using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UseComplexPropertyForContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Contact",
                table: "Trainers",
                newName: "Contact_Value");

            migrationBuilder.RenameColumn(
                name: "Contact",
                table: "Clients",
                newName: "Contact_Value");

            migrationBuilder.AddColumn<string>(
                name: "Contact_Type",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact_Type",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Contact_Type",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "Contact_Value",
                table: "Trainers",
                newName: "Contact");

            migrationBuilder.RenameColumn(
                name: "Contact_Value",
                table: "Clients",
                newName: "Contact");
        }
    }
}
