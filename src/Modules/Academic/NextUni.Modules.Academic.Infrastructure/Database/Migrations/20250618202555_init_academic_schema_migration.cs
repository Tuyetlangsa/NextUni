using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextUni.Modules.Academic.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class init_academic_schema_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "subjectGroups",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subject_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subjects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "universities",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    region = table.Column<byte>(type: "smallint", nullable: false),
                    university_type = table.Column<byte>(type: "smallint", nullable: false),
                    address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    website_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    facebook_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_universities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subject_subject_group",
                schema: "academic",
                columns: table => new
                {
                    subject_groups_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subjects_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subject_subject_group", x => new { x.subject_groups_id, x.subjects_id });
                    table.ForeignKey(
                        name: "fk_subject_subject_group_subject_groups_subject_groups_id",
                        column: x => x.subject_groups_id,
                        principalSchema: "academic",
                        principalTable: "subjectGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_subject_subject_group_subjects_subjects_id",
                        column: x => x.subjects_id,
                        principalSchema: "academic",
                        principalTable: "subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "majors",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    university_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_majors", x => x.id);
                    table.ForeignKey(
                        name: "fk_majors_universities_university_id",
                        column: x => x.university_id,
                        principalSchema: "academic",
                        principalTable: "universities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "admission_exam_scores",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    major_id = table.Column<Guid>(type: "uuid", nullable: false),
                    year = table.Column<DateOnly>(type: "date", nullable: false),
                    score = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_admission_exam_scores", x => x.id);
                    table.ForeignKey(
                        name: "fk_admission_exam_scores_majors_major_id",
                        column: x => x.major_id,
                        principalSchema: "academic",
                        principalTable: "majors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "admission_gpa_scores",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    major_id = table.Column<Guid>(type: "uuid", nullable: false),
                    year = table.Column<DateOnly>(type: "date", nullable: false),
                    score = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_admission_gpa_scores", x => x.id);
                    table.ForeignKey(
                        name: "fk_admission_gpa_scores_majors_major_id",
                        column: x => x.major_id,
                        principalSchema: "academic",
                        principalTable: "majors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "majorSubjectGroupByYears",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    major_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    year = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_major_subject_group_by_years", x => x.id);
                    table.ForeignKey(
                        name: "fk_major_subject_group_by_years_majors_major_id",
                        column: x => x.major_id,
                        principalSchema: "academic",
                        principalTable: "majors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_major_subject_group_by_years_subject_groups_subject_group_id",
                        column: x => x.subject_group_id,
                        principalSchema: "academic",
                        principalTable: "subjectGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_admission_exam_scores_major_id_year",
                schema: "academic",
                table: "admission_exam_scores",
                columns: new[] { "major_id", "year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_admission_gpa_scores_major_id_year",
                schema: "academic",
                table: "admission_gpa_scores",
                columns: new[] { "major_id", "year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_majors_code_university_id",
                schema: "academic",
                table: "majors",
                columns: new[] { "code", "university_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_majors_university_id",
                schema: "academic",
                table: "majors",
                column: "university_id");

            migrationBuilder.CreateIndex(
                name: "ix_major_subject_group_by_years_major_id_subject_group_id_year",
                schema: "academic",
                table: "majorSubjectGroupByYears",
                columns: new[] { "major_id", "subject_group_id", "year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_major_subject_group_by_years_subject_group_id",
                schema: "academic",
                table: "majorSubjectGroupByYears",
                column: "subject_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_subject_subject_group_subjects_id",
                schema: "academic",
                table: "subject_subject_group",
                column: "subjects_id");

            migrationBuilder.CreateIndex(
                name: "ix_subject_groups_code",
                schema: "academic",
                table: "subjectGroups",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_universities_code",
                schema: "academic",
                table: "universities",
                column: "code",
                unique: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admission_exam_scores",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "admission_gpa_scores",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "majorSubjectGroupByYears",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "subject_subject_group",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "majors",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "subjectGroups",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "subjects",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "universities",
                schema: "academic");
        }
    }
}
