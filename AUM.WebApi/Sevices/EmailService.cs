using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly SmtpClient _smtpClient;

    public EmailService()
    {
        _smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("alifc24p.4125@gmail.com", "vthe suzu zbjg bshg "),
            EnableSsl = true,
            Timeout = 1000,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            
            

        };
    }

    public void SendEmailAsync(string to, string subject, string body)
    {
        try {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("alifc24p.4125@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);

             _smtpClient.SendMailAsync(mailMessage);
        }

        catch (SmtpException smtpEx)
        {
            // Handle SMTP exceptions here
            Console.WriteLine($"SMTP Error: {smtpEx.Message}");
        }

    }
}
