using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextUni.Modules.Academic.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class staff_university_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "staff_account_id",
                schema: "academic",
                table: "universities",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_universities_staff_account_id",
                schema: "academic",
                table: "universities",
                column: "staff_account_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_universities_staff_account_id",
                schema: "academic",
                table: "universities");

            migrationBuilder.DropColumn(
                name: "staff_account_id",
                schema: "academic",
                table: "universities");
        }
    }
}
