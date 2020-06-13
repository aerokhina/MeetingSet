using System;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using MeetingSet.Data;
using MeetingSet.Models;

namespace MeetingSet.Services
{
  public class EmailService
  {
    public async Task SendEmailAsync(string email, Meeting meeting)
    {
      var emailMessage = new MimeMessage();
 
      emailMessage.From.Add(new MailboxAddress("Erokhina Test Mail Service", "login@yandex.ru"));
      emailMessage.To.Add(new MailboxAddress("", email));
      emailMessage.Subject = "You have been added to meeting";
      emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
      {
        Text = $"You have been added to meeting {meeting.Name}. Meeting will start at {meeting.StartDateTimeMeeting:g}" + 
                $"and ending at {meeting.EndDateTimeMeeting:g}"
      };
      
      emailMessage.WriteTo ("mails\\" + DateTime.Now.ToString("O")
        .Replace(".", "-").Replace(":", "-"));
    }
  }
}
