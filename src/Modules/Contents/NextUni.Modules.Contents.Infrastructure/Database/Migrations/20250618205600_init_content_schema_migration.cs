using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextUni.Modules.Contents.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class init_content_schema_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "counselling_articles",
                schema: "contents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    university_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    publish_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    type = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_counselling_articles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "introduction_blogs",
                schema: "contents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    introduction_type = table.Column<byte>(type: "smallint", nullable: false),
                    university_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_introduction_blogs", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "counselling_articles",
                schema: "contents");

            migrationBuilder.DropTable(
                name: "introduction_blogs",
                schema: "contents");
        }
    }
}
