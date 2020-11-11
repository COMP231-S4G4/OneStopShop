using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OneStopShop.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(nullable: false),
                    ProductDescription = table.Column<string>(nullable: false),
                    ProductPrice = table.Column<decimal>(nullable: false),
                    ProductCreatedDate = table.Column<DateTime>(nullable: false),
                    ProductModifiedDate = table.Column<DateTime>(nullable: false),
                    ProductQuantity = table.Column<int>(nullable: false),
                    ProductImage = table.Column<string>(nullable: true),
                    ProductSize = table.Column<string>(nullable: true),
                    ProductColor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    StoreId = table.Column<string>(nullable: false),
                    StoreName = table.Column<string>(nullable: true),
                    SellerFirstname = table.Column<string>(nullable: true),
                    SellerLasttname = table.Column<string>(nullable: true),
                    StoreDescription = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.StoreId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
