using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TL.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToRoomNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rooms_Number",
                table: "Rooms",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rooms_Number",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");
        }
    }
}
