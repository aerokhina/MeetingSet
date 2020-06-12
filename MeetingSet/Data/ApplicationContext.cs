using Microsoft.EntityFrameworkCore;

namespace MeetingSet.Data
{
  public class ApplicationContext : DbContext
  {
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
      : base(options)
    {
    }

    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Participant> Participants { get; set; }

    public DbSet<MeetingParticipant> MeetingParticipants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<MeetingParticipant>()
        .HasKey(t => new {t.MeetingId, t.ParticipantId});
    }
  }
}
