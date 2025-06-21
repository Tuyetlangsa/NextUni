using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextUni.Modules.Contents.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class remove_introduction_blog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "introduction_blogs",
                schema: "contents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "introduction_blogs",
                schema: "contents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    introduction_type = table.Column<byte>(type: "smallint", nullable: false),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    university_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_introduction_blogs", x => x.id);
                });
        }
    }
}
