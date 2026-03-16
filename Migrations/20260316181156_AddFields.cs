using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NC_26.Migrations
{
    /// <inheritdoc />
    public partial class AddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FieldId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_FieldId",
                table: "Groups",
                column: "FieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Fields_FieldId",
                table: "Groups",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Fields_FieldId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.DropIndex(
                name: "IX_Groups_FieldId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "Groups");
        }
    }
}
