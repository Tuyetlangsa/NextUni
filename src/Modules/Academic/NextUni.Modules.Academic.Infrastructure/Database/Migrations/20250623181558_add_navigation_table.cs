using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextUni.Modules.Academic.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_navigation_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_major_subject_group_by_year_major_id_subject_group_id_year",
                schema: "academic",
                table: "major_subject_group_by_year");

            migrationBuilder.CreateIndex(
                name: "ix_major_subject_group_by_year_major_id",
                schema: "academic",
                table: "major_subject_group_by_year",
                column: "major_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_major_subject_group_by_year_major_id",
                schema: "academic",
                table: "major_subject_group_by_year");

            migrationBuilder.CreateIndex(
                name: "ix_major_subject_group_by_year_major_id_subject_group_id_year",
                schema: "academic",
                table: "major_subject_group_by_year",
                columns: new[] { "major_id", "subject_group_id", "year" },
                unique: true);
        }
    }
}
