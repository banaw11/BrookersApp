using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class EntitiesFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnreadMessages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friends",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_UserId",
                table: "Friends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Connections",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "FriendName",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Connections");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Friends",
                newName: "IsConfirmed");

            migrationBuilder.AlterColumn<bool>(
                name: "IsConfirmed",
                table: "Friends",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "Connections",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friends",
                table: "Friends",
                columns: new[] { "UserId", "FriendId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Connections",
                table: "Connections",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FriendId",
                table: "Friends",
                column: "FriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_FriendId",
                table: "Friends",
                column: "FriendId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_FriendId",
                table: "Friends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friends",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_FriendId",
                table: "Friends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Connections",
                table: "Connections");

            migrationBuilder.RenameColumn(
                name: "IsConfirmed",
                table: "Friends",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Friends",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Friends",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FriendName",
                table: "Friends",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "Connections",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Connections",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friends",
                table: "Friends",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Connections",
                table: "Connections",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnreadMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageId = table.Column<int>(type: "INTEGER", nullable: false),
                    NotificationId = table.Column<int>(type: "INTEGER", nullable: false),
                    SenderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnreadMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnreadMessages_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friends_UserId",
                table: "Friends",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnreadMessages_NotificationId",
                table: "UnreadMessages",
                column: "NotificationId");
        }
    }
}
