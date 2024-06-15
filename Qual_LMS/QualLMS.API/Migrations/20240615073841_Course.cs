using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QualLMS.API.Migrations
{
    /// <inheritdoc />
    public partial class Course : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInformation_AspNetUsers_AppUserId",
                table: "UserInformation");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "UserInformation",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserInformation_AppUserId",
                table: "UserInformation",
                newName: "IX_UserInformation_UserId");

            migrationBuilder.AddColumn<int>(
                name: "CourseFees",
                table: "UserInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseFees",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Calendar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calendar_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calendar_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FeesReceived",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiptDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ReceiptFees = table.Column<int>(type: "int", nullable: false),
                    Mode = table.Column<int>(type: "int", nullable: false),
                    ModeDetails = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeesReceived", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeesReceived_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeesReceived_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FeesReceived_Fees_FeesId",
                        column: x => x.FeesId,
                        principalTable: "Fees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_CourseId",
                table: "Calendar",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_UserId",
                table: "Calendar",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeesReceived_CourseId",
                table: "FeesReceived",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_FeesReceived_FeesId",
                table: "FeesReceived",
                column: "FeesId");

            migrationBuilder.CreateIndex(
                name: "IX_FeesReceived_UserId",
                table: "FeesReceived",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformation_AspNetUsers_UserId",
                table: "UserInformation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInformation_AspNetUsers_UserId",
                table: "UserInformation");

            migrationBuilder.DropTable(
                name: "Calendar");

            migrationBuilder.DropTable(
                name: "FeesReceived");

            migrationBuilder.DropColumn(
                name: "CourseFees",
                table: "UserInformation");

            migrationBuilder.DropColumn(
                name: "CourseFees",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserInformation",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserInformation_UserId",
                table: "UserInformation",
                newName: "IX_UserInformation_AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformation_AspNetUsers_AppUserId",
                table: "UserInformation",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
