using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvironSofwateTest.DataAccess.Migrations
{
    public partial class MigrationFourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Reviews_ReviewId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ReviewId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Employees");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_EmployeeId",
                table: "Reviews",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Employees_EmployeeId",
                table: "Reviews",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Employees_EmployeeId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_EmployeeId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Reviews");

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ReviewId",
                table: "Employees",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Reviews_ReviewId",
                table: "Employees",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
