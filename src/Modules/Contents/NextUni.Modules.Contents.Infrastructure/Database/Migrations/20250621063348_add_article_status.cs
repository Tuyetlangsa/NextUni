using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextUni.Modules.Contents.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_article_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                schema: "contents",
                table: "counselling_articles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                schema: "contents",
                table: "counselling_articles");
        }
    }
}
