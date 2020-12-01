using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OneStopShop.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.CreateTable(
                name: "CustomOrders",
                columns: table => new
                {
                    CustomOrderID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(nullable: false),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    PhoneNum = table.Column<string>(nullable: true),
                    OrderCreatedDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ProductType = table.Column<string>(nullable: true),
                    Chest = table.Column<string>(nullable: true),
                    Neck = table.Column<string>(nullable: true),
                    Shoulder = table.Column<string>(nullable: true),
                    Sleeve = table.Column<string>(nullable: true),
                    Waist = table.Column<string>(nullable: true),
                    Hip = table.Column<string>(nullable: true),
                    InseamLength = table.Column<string>(nullable: true),
                    FullLength = table.Column<string>(nullable: true),
                    AnkleLength = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomOrders", x => x.CustomOrderID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomOrders");

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                });
        }
    }
}
