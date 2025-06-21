using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextUni.Modules.Academic.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class fix_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "university_id",
                schema: "academic",
                table: "introduction_blogs");

            migrationBuilder.AddColumn<Guid>(
                name: "target_id",
                schema: "academic",
                table: "introduction_blogs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "target_id",
                schema: "academic",
                table: "introduction_blogs");

            migrationBuilder.AddColumn<Guid>(
                name: "university_id",
                schema: "academic",
                table: "introduction_blogs",
                type: "uuid",
                nullable: true);
        }
    }
}
