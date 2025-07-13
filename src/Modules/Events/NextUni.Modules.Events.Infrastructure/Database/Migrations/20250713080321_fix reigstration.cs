using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextUni.Modules.Events.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class fixreigstration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                schema: "events",
                table: "event_registrations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "status",
                schema: "events",
                table: "event_registrations",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }
    }
}
