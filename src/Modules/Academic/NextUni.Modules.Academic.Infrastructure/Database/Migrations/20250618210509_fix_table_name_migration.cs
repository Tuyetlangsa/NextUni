using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextUni.Modules.Academic.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class fix_table_name_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_major_subject_group_by_years_majors_major_id",
                schema: "academic",
                table: "majorSubjectGroupByYears");

            migrationBuilder.DropForeignKey(
                name: "fk_major_subject_group_by_years_subject_groups_subject_group_id",
                schema: "academic",
                table: "majorSubjectGroupByYears");

            migrationBuilder.DropPrimaryKey(
                name: "pk_major_subject_group_by_years",
                schema: "academic",
                table: "majorSubjectGroupByYears");

            migrationBuilder.RenameTable(
                name: "subjectGroups",
                schema: "academic",
                newName: "subject_groups",
                newSchema: "academic");

            migrationBuilder.RenameTable(
                name: "majorSubjectGroupByYears",
                schema: "academic",
                newName: "major_subject_group_by_year",
                newSchema: "academic");

            migrationBuilder.RenameIndex(
                name: "ix_major_subject_group_by_years_subject_group_id",
                schema: "academic",
                table: "major_subject_group_by_year",
                newName: "ix_major_subject_group_by_year_subject_group_id");

            migrationBuilder.RenameIndex(
                name: "ix_major_subject_group_by_years_major_id_subject_group_id_year",
                schema: "academic",
                table: "major_subject_group_by_year",
                newName: "ix_major_subject_group_by_year_major_id_subject_group_id_year");

            migrationBuilder.AddPrimaryKey(
                name: "pk_major_subject_group_by_year",
                schema: "academic",
                table: "major_subject_group_by_year",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_major_subject_group_by_year_majors_major_id",
                schema: "academic",
                table: "major_subject_group_by_year",
                column: "major_id",
                principalSchema: "academic",
                principalTable: "majors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_major_subject_group_by_year_subject_groups_subject_group_id",
                schema: "academic",
                table: "major_subject_group_by_year",
                column: "subject_group_id",
                principalSchema: "academic",
                principalTable: "subject_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_major_subject_group_by_year_majors_major_id",
                schema: "academic",
                table: "major_subject_group_by_year");

            migrationBuilder.DropForeignKey(
                name: "fk_major_subject_group_by_year_subject_groups_subject_group_id",
                schema: "academic",
                table: "major_subject_group_by_year");

            migrationBuilder.DropPrimaryKey(
                name: "pk_major_subject_group_by_year",
                schema: "academic",
                table: "major_subject_group_by_year");

            migrationBuilder.RenameTable(
                name: "subject_groups",
                schema: "academic",
                newName: "subjectGroups",
                newSchema: "academic");

            migrationBuilder.RenameTable(
                name: "major_subject_group_by_year",
                schema: "academic",
                newName: "majorSubjectGroupByYears",
                newSchema: "academic");

            migrationBuilder.RenameIndex(
                name: "ix_major_subject_group_by_year_subject_group_id",
                schema: "academic",
                table: "majorSubjectGroupByYears",
                newName: "ix_major_subject_group_by_years_subject_group_id");

            migrationBuilder.RenameIndex(
                name: "ix_major_subject_group_by_year_major_id_subject_group_id_year",
                schema: "academic",
                table: "majorSubjectGroupByYears",
                newName: "ix_major_subject_group_by_years_major_id_subject_group_id_year");

            migrationBuilder.AddPrimaryKey(
                name: "pk_major_subject_group_by_years",
                schema: "academic",
                table: "majorSubjectGroupByYears",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_major_subject_group_by_years_majors_major_id",
                schema: "academic",
                table: "majorSubjectGroupByYears",
                column: "major_id",
                principalSchema: "academic",
                principalTable: "majors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_major_subject_group_by_years_subject_groups_subject_group_id",
                schema: "academic",
                table: "majorSubjectGroupByYears",
                column: "subject_group_id",
                principalSchema: "academic",
                principalTable: "subjectGroups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
