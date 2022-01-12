using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IWantApp.Migrations
{
    public partial class AddCategoryAndProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    has_active = table.Column<bool>(type: "bit", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    edited_by = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    edited_on = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    has_stock = table.Column<bool>(type: "bit", nullable: false),
                    has_active = table.Column<bool>(type: "bit", nullable: false),
                    category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    edited_by = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    edited_on = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_products_tbl_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "tbl_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_products_category_id",
                table: "tbl_products",
                column: "category_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_products");

            migrationBuilder.DropTable(
                name: "tbl_categories");
        }
    }
}
