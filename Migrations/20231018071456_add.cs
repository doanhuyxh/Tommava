using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tommava.Migrations
{
    public partial class add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameVn",
                table: "Video",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Video",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "Video",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Video",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriesId = table.Column<int>(type: "int", nullable: true),
                    SubName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropColumn(
                name: "NameVn",
                table: "Video");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Video");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Video");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Video");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Category");
        }
    }
}
