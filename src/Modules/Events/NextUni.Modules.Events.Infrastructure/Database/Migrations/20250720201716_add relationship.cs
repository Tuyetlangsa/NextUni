using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextUni.Modules.Events.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class addrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_event_registrations_event_id",
                schema: "events",
                table: "event_registrations",
                column: "event_id");

            migrationBuilder.AddForeignKey(
                name: "fk_event_registrations_events_event_id",
                schema: "events",
                table: "event_registrations",
                column: "event_id",
                principalSchema: "events",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_event_registrations_events_event_id",
                schema: "events",
                table: "event_registrations");

            migrationBuilder.DropIndex(
                name: "ix_event_registrations_event_id",
                schema: "events",
                table: "event_registrations");
        }
    }
}
