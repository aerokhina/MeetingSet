using Microsoft.EntityFrameworkCore;

namespace MeetingSet.Data
{
  public class ApplicationContext : DbContext
  {
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
      : base(options)
    {
      Database.EnsureCreated();
    }
    
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Participant> Participants { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<MeetingParticipant>()
        .HasKey(t => new { t.MeetingId, t.ParticipantId });
 
      modelBuilder.Entity<MeetingParticipant>()
        .HasOne(sc => sc.Meeting)
        .WithMany(s => s.MeetingParticipants)
        .HasForeignKey(sc => sc.MeetingId);
 
      modelBuilder.Entity<MeetingParticipant>()
        .HasOne(sc => sc.Participant)
        .WithMany(c => c.MeetingParticipants)
        .HasForeignKey(sc => sc.ParticipantId);
    }
  }
}
