using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Flights_FlightIdFK",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_FlightIdFK",
                table: "Tickets");

            migrationBuilder.AlterColumn<double>(
                name: "PricePaid",
                table: "Tickets",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PricePaid",
                table: "Tickets",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FlightIdFK",
                table: "Tickets",
                column: "FlightIdFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Flights_FlightIdFK",
                table: "Tickets",
                column: "FlightIdFK",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
