namespace FoodDelivery.Services;

using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpSettings = _configuration.GetSection("SmtpSettings");

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(smtpSettings["SenderName"], smtpSettings["SenderEmail"]));
        email.To.Add(new MailboxAddress("", toEmail));
        email.Subject = subject;

        email.Body = new TextPart("html")
        {
            Text = $"<pre>{body}</pre>"
        };

        using var smtp = new SmtpClient();

        try
        {
            await smtp.ConnectAsync(smtpSettings["Server"], int.Parse(smtpSettings["Port"]),
                MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(smtpSettings["Username"], smtpSettings["Password"]);
            await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
}
