using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetingSet.Migrations
{
    public partial class ChangeMeetings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Participants_ParticipantId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_ParticipantId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "ParticipantId",
                table: "Meetings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParticipantId",
                table: "Meetings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_ParticipantId",
                table: "Meetings",
                column: "ParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Participants_ParticipantId",
                table: "Meetings",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
