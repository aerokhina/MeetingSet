using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using MailKit.Security;
using MeetingSet.Data;
using Microsoft.Extensions.Options;

namespace MeetingSet.Services
{
  public class EmailService
    : IEmailService
  {
    private readonly EmailConfiguration _emailConfiguration;

    public EmailService(IOptions<EmailConfiguration> emailConfiguration)
    {
      _emailConfiguration = emailConfiguration.Value;
    }

    public async Task SendEmailAsync(string email, Meeting meeting)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("", _emailConfiguration.From));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = "You have been added to meeting";
        emailMessage.Body =new TextPart(MimeKit.Text.TextFormat.Html)
        {
          Text =
            $"You have been added to meeting {meeting.Name}. Meeting will start at {meeting.StartDateTimeMeeting:g} " +
            $"and will end at {meeting.EndDateTimeMeeting:g}"
        };
        
        using (SmtpClient client = new SmtpClient())
        {
          await client.ConnectAsync(_emailConfiguration.Host, _emailConfiguration.Port, SecureSocketOptions.StartTls);
          await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);
          await client.SendAsync(emailMessage);

          await client.DisconnectAsync(true);
        }
    }
  }
}