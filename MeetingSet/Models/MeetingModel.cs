using System;
using System.Collections.Generic;
using MeetingSet.Data;

namespace MeetingSet.Models
{
  public class MeetingInputModel
  {
    public string Name { get; set; }
    
    public DateTime DateTimeMeeting { get; set; }
  }
  
  public class MeetingOutputModel
  {
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime DateTimeMeeting { get; set; }
  }
}
