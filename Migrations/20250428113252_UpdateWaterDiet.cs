using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWaterDiet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "WaterDiet",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WaterDiet_UserId",
                table: "WaterDiet",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WaterDiet_Users_UserId",
                table: "WaterDiet",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaterDiet_Users_UserId",
                table: "WaterDiet");

            migrationBuilder.DropIndex(
                name: "IX_WaterDiet_UserId",
                table: "WaterDiet");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WaterDiet");
        }
    }
}
