using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CompanyName = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    TaxNo = table.Column<string>(type: "varchar(13)", maxLength: 13, nullable: true),
                    Phone = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Fax = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Line = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Facebook = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Website = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true),
                    Logo = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true),
                    Comment = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                    Created_By_Id = table.Column<int>(type: "int", nullable: false),
                    Creation_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Last_Update_By_Id = table.Column<int>(type: "int", nullable: false),
                    Last_Update_By_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_CompanyId",
                table: "User",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Company_CompanyId",
                table: "User",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Company_CompanyId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropIndex(
                name: "IX_User_CompanyId",
                table: "User");
        }
    }
}
