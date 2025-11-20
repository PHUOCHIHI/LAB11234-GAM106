using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using WebApplication1.Models;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Service;

public class EmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(EmailSettings emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(EmailRequest emailRequest)
    {
        try
        {
            // Validate email settings
            if (string.IsNullOrEmpty(_emailSettings.SmtpServer) || 
                string.IsNullOrEmpty(_emailSettings.SenderEmail) ||
                string.IsNullOrEmpty(_emailSettings.Username) ||
                string.IsNullOrEmpty(_emailSettings.Password))
            {
                _logger.LogError("Email settings are not configured properly");
                return false;
            }

            // Validate email request
            if (string.IsNullOrEmpty(emailRequest.To))
            {
                _logger.LogError("Email recipient is required");
                return false;
            }

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
                _logger.LogInformation($"Connecting to SMTP server: {_emailSettings.SmtpServer}:{_emailSettings.SmtpPort}");
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                _logger.LogInformation($"Email sent successfully to {emailRequest.To}");
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending email to {emailRequest.To}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var emailRequest = new EmailRequest
            {
                To = to,
                Subject = subject,
                Body = body,
                IsHtml = false
            };
            return await SendEmailAsync(emailRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending email to {to}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SendHtmlEmailAsync(string to, string subject, string htmlBody)
    {
        try
        {
            var emailRequest = new EmailRequest
            {
                To = to,
                Subject = subject,
                Body = htmlBody,
                IsHtml = true
            };
            return await SendEmailAsync(emailRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending HTML email to {to}: {ex.Message}");
            return false;
        }
    }
}

