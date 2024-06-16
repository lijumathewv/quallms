using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QualLMS.API.Migrations
{
    /// <inheritdoc />
    public partial class Calendar01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Calendar",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_OrganizationId",
                table: "Calendar",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calendar_Organization_OrganizationId",
                table: "Calendar",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calendar_Organization_OrganizationId",
                table: "Calendar");

            migrationBuilder.DropIndex(
                name: "IX_Calendar_OrganizationId",
                table: "Calendar");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Calendar");
        }
    }
}
