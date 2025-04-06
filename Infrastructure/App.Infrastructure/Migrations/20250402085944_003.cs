using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserPasswordLog",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "UserPasswordLog",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UserPasswordLog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "UserPasswordLog",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "UserPasswordLog",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserLoginLog",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserLoginLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("Relational:ColumnOrder", 0)
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "UserLoginLog",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UserLoginLog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "UserLoginLog",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "UserLoginLog",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPasswordLog_CreatedById",
                table: "UserPasswordLog",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPasswordLog_LastModifiedById",
                table: "UserPasswordLog",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPasswordLog_UserId",
                table: "UserPasswordLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginLog_CreatedById",
                table: "UserLoginLog",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginLog_LastModifiedById",
                table: "UserLoginLog",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginLog_UserId",
                table: "UserLoginLog",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLoginLog_Users_CreatedById",
                table: "UserLoginLog",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLoginLog_Users_LastModifiedById",
                table: "UserLoginLog",
                column: "LastModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLoginLog_Users_UserId",
                table: "UserLoginLog",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPasswordLog_Users_CreatedById",
                table: "UserPasswordLog",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPasswordLog_Users_LastModifiedById",
                table: "UserPasswordLog",
                column: "LastModifiedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPasswordLog_Users_UserId",
                table: "UserPasswordLog",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLoginLog_Users_CreatedById",
                table: "UserLoginLog");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLoginLog_Users_LastModifiedById",
                table: "UserLoginLog");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLoginLog_Users_UserId",
                table: "UserLoginLog");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPasswordLog_Users_CreatedById",
                table: "UserPasswordLog");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPasswordLog_Users_LastModifiedById",
                table: "UserPasswordLog");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPasswordLog_Users_UserId",
                table: "UserPasswordLog");

            migrationBuilder.DropIndex(
                name: "IX_UserPasswordLog_CreatedById",
                table: "UserPasswordLog");

            migrationBuilder.DropIndex(
                name: "IX_UserPasswordLog_LastModifiedById",
                table: "UserPasswordLog");

            migrationBuilder.DropIndex(
                name: "IX_UserPasswordLog_UserId",
                table: "UserPasswordLog");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginLog_CreatedById",
                table: "UserLoginLog");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginLog_LastModifiedById",
                table: "UserLoginLog");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginLog_UserId",
                table: "UserLoginLog");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "UserPasswordLog");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UserPasswordLog");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "UserPasswordLog");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "UserPasswordLog");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "UserLoginLog");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UserLoginLog");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "UserLoginLog");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "UserLoginLog");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserPasswordLog",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserLoginLog",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserLoginLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0)
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
