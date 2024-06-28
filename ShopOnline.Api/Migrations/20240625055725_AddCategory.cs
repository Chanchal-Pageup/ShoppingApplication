using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopOnline.Api.Migrations
{
    public partial class AddCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "IconCSS", "Name" },
                values: new object[] { 5, "fas fa-cloth", "Men" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
