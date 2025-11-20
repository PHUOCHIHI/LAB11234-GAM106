using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using WebApplication1.Models;

namespace WebApplication1.Service;

public class EmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(EmailSettings emailSettings)
    {
        _emailSettings = emailSettings;
    }

    public async Task<bool> SendEmailAsync(EmailRequest emailRequest)
    {
        try
        {
            // Tạo message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(emailRequest.To));
            message.Subject = emailRequest.Subject;

            // Tạo body
            var bodyBuilder = new BodyBuilder();
            if (emailRequest.IsHtml)
            {
                bodyBuilder.HtmlBody = emailRequest.Body;
            }
            else
            {
                bodyBuilder.TextBody = emailRequest.Body;
            }
            message.Body = bodyBuilder.ToMessageBody();

            // Gửi email
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string message)
    {
        try
        {
            var emailRequest = new EmailRequest
            {
                To = to,
                Subject = subject,
                Body = message,
                IsHtml = false
            };
            return await SendEmailAsync(emailRequest);
        }
        catch (Exception)
        {
            return false;
        }
    }
}

