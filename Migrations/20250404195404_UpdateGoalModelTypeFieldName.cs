using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGoalModelTypeFieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_GoalTypes_typeId",
                table: "Goals");

            migrationBuilder.RenameColumn(
                name: "typeId",
                table: "Goals",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Goals_typeId",
                table: "Goals",
                newName: "IX_Goals_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_GoalTypes_TypeId",
                table: "Goals",
                column: "TypeId",
                principalTable: "GoalTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_GoalTypes_TypeId",
                table: "Goals");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Goals",
                newName: "typeId");

            migrationBuilder.RenameIndex(
                name: "IX_Goals_TypeId",
                table: "Goals",
                newName: "IX_Goals_typeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_GoalTypes_typeId",
                table: "Goals",
                column: "typeId",
                principalTable: "GoalTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
