﻿// <auto-generated />
using System;
using MeetingSet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MeetingSet.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20200610205027_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("MeetingSet.Data.Meeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("DateTimeMeeting")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("ParticipantId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParticipantId");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("MeetingSet.Data.MeetingParticipant", b =>
                {
                    b.Property<int>("MeetingId")
                        .HasColumnType("integer");

                    b.Property<int>("ParticipantId")
                        .HasColumnType("integer");

                    b.HasKey("MeetingId", "ParticipantId");

                    b.HasIndex("ParticipantId");

                    b.ToTable("MeetingParticipant");
                });

            modelBuilder.Entity("MeetingSet.Data.Participant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("MeetingSet.Data.Meeting", b =>
                {
                    b.HasOne("MeetingSet.Data.Participant", null)
                        .WithMany("Meetings")
                        .HasForeignKey("ParticipantId");
                });

            modelBuilder.Entity("MeetingSet.Data.MeetingParticipant", b =>
                {
                    b.HasOne("MeetingSet.Data.Meeting", "Meeting")
                        .WithMany("MeetingParticipants")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MeetingSet.Data.Participant", "Participant")
                        .WithMany("MeetingParticipants")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
