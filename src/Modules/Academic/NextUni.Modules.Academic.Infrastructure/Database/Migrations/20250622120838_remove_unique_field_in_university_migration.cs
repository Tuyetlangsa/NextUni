using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextUni.Modules.Academic.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class remove_unique_field_in_university_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_universities_facebook_url",
                schema: "academic",
                table: "universities");

            migrationBuilder.DropIndex(
                name: "ix_universities_website_url",
                schema: "academic",
                table: "universities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_universities_facebook_url",
                schema: "academic",
                table: "universities",
                column: "facebook_url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_universities_website_url",
                schema: "academic",
                table: "universities",
                column: "website_url",
                unique: true);
        }
    }
}
