using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetingSet.Migrations
{
    public partial class AddMeetingParticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingParticipant_Meetings_MeetingId",
                table: "MeetingParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingParticipant_Participants_ParticipantId",
                table: "MeetingParticipant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingParticipant",
                table: "MeetingParticipant");

            migrationBuilder.RenameTable(
                name: "MeetingParticipant",
                newName: "MeetingParticipants");

            migrationBuilder.RenameIndex(
                name: "IX_MeetingParticipant_ParticipantId",
                table: "MeetingParticipants",
                newName: "IX_MeetingParticipants_ParticipantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingParticipants",
                table: "MeetingParticipants",
                columns: new[] { "MeetingId", "ParticipantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingParticipants_Meetings_MeetingId",
                table: "MeetingParticipants",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingParticipants_Participants_ParticipantId",
                table: "MeetingParticipants",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingParticipants_Meetings_MeetingId",
                table: "MeetingParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingParticipants_Participants_ParticipantId",
                table: "MeetingParticipants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingParticipants",
                table: "MeetingParticipants");

            migrationBuilder.RenameTable(
                name: "MeetingParticipants",
                newName: "MeetingParticipant");

            migrationBuilder.RenameIndex(
                name: "IX_MeetingParticipants_ParticipantId",
                table: "MeetingParticipant",
                newName: "IX_MeetingParticipant_ParticipantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingParticipant",
                table: "MeetingParticipant",
                columns: new[] { "MeetingId", "ParticipantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingParticipant_Meetings_MeetingId",
                table: "MeetingParticipant",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingParticipant_Participants_ParticipantId",
                table: "MeetingParticipant",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
