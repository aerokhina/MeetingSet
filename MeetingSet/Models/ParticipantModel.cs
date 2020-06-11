namespace MeetingSet.Models
{
  public class ParticipantInputModel
  {
    public string Name { get; set; }
    
    public string Email { get; set; }
  }
  
  public class ParticipantOutputModel
  {
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
  }

  public class ParticipantIdModel
  {
    public int Id { get; set; }
  }
}
