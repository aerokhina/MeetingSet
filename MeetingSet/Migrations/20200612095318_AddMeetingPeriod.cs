using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetingSet.Migrations
{
    public partial class AddMeetingPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeMeeting",
                table: "Meetings");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTimeMeeting",
                table: "Meetings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTimeMeeting",
                table: "Meetings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateTimeMeeting",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "StartDateTimeMeeting",
                table: "Meetings");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeMeeting",
                table: "Meetings",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
