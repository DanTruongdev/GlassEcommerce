using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlassECommerce.Migrations
{
    public partial class editModelEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryPrice",
                table: "Models");

            migrationBuilder.RenameColumn(
                name: "SecondaryPrice",
                table: "Models",
                newName: "Price");

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 1,
                column: "Price",
                value: 500.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Models",
                newName: "SecondaryPrice");

            migrationBuilder.AddColumn<double>(
                name: "PrimaryPrice",
                table: "Models",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: 1,
                columns: new[] { "PrimaryPrice", "SecondaryPrice" },
                values: new object[] { 500.0, 400.0 });
        }
    }
}
