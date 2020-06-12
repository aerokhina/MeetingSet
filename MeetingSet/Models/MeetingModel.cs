using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingSet.Models
{
  public class MeetingInputModel
  {
    [Required]
    public string Name { get; set; }
    
    [Required]
    public DateTime? StartDateTimeMeeting { get; set; }
    
    [Required]
    public DateTime? EndDateTimeMeeting { get; set; }
  }

  public class MeetingOutputModel
  {
    public int Id { get; set; }

    public string Name { get; set; }
    
    public DateTime StartDateTimeMeeting { get; set; }
    
    public DateTime EndDateTimeMeeting { get; set; }
  }

  public class MeetingListItemModel
  {
    public int Id { get; set; }

    public string Name { get; set; }
    
    public DateTime StartDateTimeMeeting { get; set; }
    
    public DateTime EndDateTimeMeeting { get; set; }

    public List<ParticipantOutputModel> Participants { get; set; }
  }
}
