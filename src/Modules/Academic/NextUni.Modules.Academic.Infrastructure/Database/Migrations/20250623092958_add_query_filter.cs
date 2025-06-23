using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextUni.Modules.Academic.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_query_filter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                schema: "academic",
                table: "subject_groups",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                schema: "academic",
                table: "subject_groups");
        }
    }
}
