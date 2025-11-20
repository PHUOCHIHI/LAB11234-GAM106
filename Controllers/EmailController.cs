using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly EmailService _emailService;
    private readonly ILogger<EmailController> _logger;

    public EmailController(EmailService emailService, ILogger<EmailController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    /// <summary>
    /// Gửi email đơn giản (text)
    /// </summary>
    [HttpPost("Send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
    {
        try
        {
            if (string.IsNullOrEmpty(emailRequest.To) || 
                string.IsNullOrEmpty(emailRequest.Subject) || 
                string.IsNullOrEmpty(emailRequest.Body))
            {
                return BadRequest(new ResponseApi
                {
                    IsSuccess = false,
                    Notification = "Vui lòng điền đầy đủ thông tin: To, Subject, Body",
                    Data = null
                });
            }

            var result = await _emailService.SendEmailAsync(emailRequest);

            if (result)
            {
                return Ok(new ResponseApi
                {
                    IsSuccess = true,
                    Notification = "Gửi email thành công",
                    Data = new { To = emailRequest.To, Subject = emailRequest.Subject }
                });
            }
            else
            {
                return BadRequest(new ResponseApi
                {
                    IsSuccess = false,
                    Notification = "Gửi email thất bại. Vui lòng kiểm tra cấu hình email trong appsettings.json",
                    Data = null
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SendEmail endpoint");
            return BadRequest(new ResponseApi
            {
                IsSuccess = false,
                Notification = "Lỗi khi gửi email",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gửi email đơn giản với tham số riêng lẻ
    /// </summary>
    [HttpPost("SendSimple")]
    public async Task<IActionResult> SendSimpleEmail([FromBody] SimpleEmailRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.To) || 
                string.IsNullOrEmpty(request.Subject) || 
                string.IsNullOrEmpty(request.Message))
            {
                return BadRequest(new ResponseApi
                {
                    IsSuccess = false,
                    Notification = "Vui lòng điền đầy đủ thông tin: To, Subject, Message",
                    Data = null
                });
            }

            var result = await _emailService.SendEmailAsync(request.To, request.Subject, request.Message);

            if (result)
            {
                return Ok(new ResponseApi
                {
                    IsSuccess = true,
                    Notification = "Gửi email thành công",
                    Data = new { To = request.To, Subject = request.Subject }
                });
            }
            else
            {
                return BadRequest(new ResponseApi
                {
                    IsSuccess = false,
                    Notification = "Gửi email thất bại. Vui lòng kiểm tra cấu hình email trong appsettings.json",
                    Data = null
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SendSimpleEmail endpoint");
            return BadRequest(new ResponseApi
            {
                IsSuccess = false,
                Notification = "Lỗi khi gửi email",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gửi email HTML
    /// </summary>
    [HttpPost("SendHtml")]
    public async Task<IActionResult> SendHtmlEmail([FromBody] HtmlEmailRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.To) || 
                string.IsNullOrEmpty(request.Subject) || 
                string.IsNullOrEmpty(request.HtmlBody))
            {
                return BadRequest(new ResponseApi
                {
                    IsSuccess = false,
                    Notification = "Vui lòng điền đầy đủ thông tin: To, Subject, HtmlBody",
                    Data = null
                });
            }

            var result = await _emailService.SendHtmlEmailAsync(request.To, request.Subject, request.HtmlBody);

            if (result)
            {
                return Ok(new ResponseApi
                {
                    IsSuccess = true,
                    Notification = "Gửi email HTML thành công",
                    Data = new { To = request.To, Subject = request.Subject }
                });
            }
            else
            {
                return BadRequest(new ResponseApi
                {
                    IsSuccess = false,
                    Notification = "Gửi email thất bại. Vui lòng kiểm tra cấu hình email trong appsettings.json",
                    Data = null
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SendHtmlEmail endpoint");
            return BadRequest(new ResponseApi
            {
                IsSuccess = false,
                Notification = "Lỗi khi gửi email",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Test gửi email với template OTP
    /// </summary>
    [HttpPost("SendOTP")]
    public async Task<IActionResult> SendOTPEmail([FromBody] OTPEmailRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.To) || string.IsNullOrEmpty(request.OTP))
            {
                return BadRequest(new ResponseApi
                {
                    IsSuccess = false,
                    Notification = "Vui lòng điền đầy đủ thông tin: To, OTP",
                    Data = null
                });
            }

            string subject = "Reset Password Game 106 - " + request.To;
            string htmlBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif; padding: 20px;'>
                    <h2 style='color: #333;'>Mã OTP của bạn</h2>
                    <p>Xin chào,</p>
                    <p>Mã OTP để reset mật khẩu của bạn là:</p>
                    <div style='background-color: #f4f4f4; padding: 15px; text-align: center; font-size: 24px; font-weight: bold; color: #007bff; margin: 20px 0;'>
                        {request.OTP}
                    </div>
                    <p>Mã này có hiệu lực trong 10 phút.</p>
                    <p>Nếu bạn không yêu cầu reset mật khẩu, vui lòng bỏ qua email này.</p>
                    <p>Trân trọng,<br/>Game 106 Team</p>
                </body>
                </html>";

            var result = await _emailService.SendHtmlEmailAsync(request.To, subject, htmlBody);

            if (result)
            {
                return Ok(new ResponseApi
                {
                    IsSuccess = true,
                    Notification = "Gửi mã OTP thành công",
                    Data = new { To = request.To }
                });
            }
            else
            {
                return BadRequest(new ResponseApi
                {
                    IsSuccess = false,
                    Notification = "Gửi email thất bại. Vui lòng kiểm tra cấu hình email trong appsettings.json",
                    Data = null
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SendOTPEmail endpoint");
            return BadRequest(new ResponseApi
            {
                IsSuccess = false,
                Notification = "Lỗi khi gửi email",
                Data = ex.Message
            });
        }
    }
}

// DTOs cho Email Controller
public class SimpleEmailRequest
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class HtmlEmailRequest
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
}

public class OTPEmailRequest
{
    public string To { get; set; } = string.Empty;
    public string OTP { get; set; } = string.Empty;
}

