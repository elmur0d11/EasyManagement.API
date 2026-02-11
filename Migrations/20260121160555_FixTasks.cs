using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class FixTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tasks_rooms_RoomId",
                table: "tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_users_Userid",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_RoomId",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_Userid",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "Userid",
                table: "tasks");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_room_id",
                table: "tasks",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_user_id",
                table: "tasks",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_rooms_room_id",
                table: "tasks",
                column: "room_id",
                principalTable: "rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_users_user_id",
                table: "tasks",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tasks_rooms_room_id",
                table: "tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_tasks_users_user_id",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_room_id",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_user_id",
                table: "tasks");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Userid",
                table: "tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tasks_RoomId",
                table: "tasks",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_Userid",
                table: "tasks",
                column: "Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_rooms_RoomId",
                table: "tasks",
                column: "RoomId",
                principalTable: "rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_users_Userid",
                table: "tasks",
                column: "Userid",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
