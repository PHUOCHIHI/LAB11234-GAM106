using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Service;
using WebApplication1.ViewModel;
using System.IO;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class APIGameController : ControllerBase
{
    private ApplicationDbContext _db;
    private ResponseApi _response;
    private UserManager<ApplicationUser> _userManager;
    private SignInManager<ApplicationUser> _signInManager;
    private EmailService _emailService;

    public APIGameController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, EmailService emailService)
    {
        _db = db;
        _response = new ResponseApi();
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    [HttpGet("GetAllGameLevel")]
    public async Task<IActionResult> GetAllGameLevel()
    {
        try
        {
            var gameLevel = await _db.GameLevels.ToListAsync();
            _response.IsSuccess = true;
            _response.Notification = "Lấy dữ liệu thành công";
            _response.Data = gameLevel;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpGet("GetAllQuestionGame")]
    public async Task<IActionResult> GetAllQuestionGame()
    {
        try
        {
            var questions = await _db.Questions.ToListAsync();
            _response.IsSuccess = true;
            _response.Notification = "Lấy dữ liệu thành công";
            _response.Data = questions;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpGet("GetAllQuestionGameByLevel/{levelId}")]
    public async Task<IActionResult> GetAllQuestionGameByLevel(int levelId)
    {
        try
        {
            var questions = await _db.Questions
                .Where(q => q.levelId == levelId)
                .ToListAsync();
            
            _response.IsSuccess = true;
            _response.Notification = "Lấy dữ liệu thành công";
            _response.Data = questions;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpGet("GetAllRegion")]
    public async Task<IActionResult> GetAllRegion()
    {
        try
        {
            var regions = await _db.Regions.ToListAsync();
            _response.IsSuccess = true;
            _response.Notification = "Lấy dữ liệu thành công";
            _response.Data = regions;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        try
        {
            var user = new ApplicationUser
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                Name = registerDTO.Name,
                Avatar = registerDTO.LinkAvatar,
                RegionId = registerDTO.RegionId
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                _response.IsSuccess = true;
                _response.Notification = "Đăng ký thành công";
                _response.Data = user;
                return Ok(_response);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Notification = "Đăng ký thất bại";
                _response.Data = result.Errors;
                return BadRequest(_response);
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Notification = "Email hoặc mật khẩu không đúng";
                _response.Data = null;
                return BadRequest(_response);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _response.IsSuccess = true;
                _response.Notification = "Đăng nhập thành công";
                _response.Data = user;
                return Ok(_response);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Notification = "Email hoặc mật khẩu không đúng";
                _response.Data = null;
                return BadRequest(_response);
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpPost("SaveResult")]
    public async Task<IActionResult> SaveResult([FromBody] LevelResultDTO levelResultDTO)
    {
        try
        {
            var levelResult = new LevelResult
            {
                UserId = levelResultDTO.UserId,
                LevelId = levelResultDTO.LevelId,
                Score = levelResultDTO.Score,
                CompletionDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _db.LevelResults.Add(levelResult);
            await _db.SaveChangesAsync();

            _response.IsSuccess = true;
            _response.Notification = "Lưu kết quả thành công";
            _response.Data = levelResult;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpGet("Rating")]
    public async Task<IActionResult> Rating()
    {
        try
        {
            var levelResults = await _db.LevelResults.ToListAsync();
            var users = await _db.Users.ToListAsync();
            var regions = await _db.Regions.ToListAsync();

            var userResults = levelResults
                .Join(users,
                    lr => lr.UserId,
                    u => u.Id,
                    (lr, u) => new
                    {
                        UserId = u.Id,
                        NameUser = u.Name,
                        RegionId = u.RegionId,
                        Score = lr.Score,
                        LevelId = lr.LevelId
                    })
                .GroupBy(x => new { x.UserId, x.NameUser, x.RegionId })
                .Select(g => new UserResultSum
                {
                    NameUser = g.Key.NameUser,
                    SumScore = g.Sum(x => x.Score),
                    SumLevel = g.Select(x => x.LevelId).Distinct().Count()
                })
                .ToList();

            var ratingData = regions
                .Select(r => new RatingVM
                {
                    NameRegion = r.Name,
                    userResultSums = userResults
                        .Where(ur => users.Any(u => u.Name == ur.NameUser && u.RegionId == r.RegionId))
                        .OrderByDescending(ur => ur.SumScore)
                        .ToList()
                })
                .ToList();

            _response.IsSuccess = true;
            _response.Notification = "Lấy dữ liệu thành công";
            _response.Data = ratingData;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpGet("GetUserInformation/{userId}")]
    public async Task<IActionResult> GetUserInformation(string userId)
    {
        try
        {
            var user = await _db.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Notification = "Không tìm thấy người dùng";
                _response.Data = null;
                return BadRequest(_response);
            }

            UserInformationVM userInformationVM = new();
            userInformationVM.Name = user.Name;
            userInformationVM.Email = user.Email ?? string.Empty;
            userInformationVM.avatar = user.Avatar;
            userInformationVM.Region = await _db.Regions.Where(x => x.RegionId == user.RegionId).Select(x => x.Name).FirstOrDefaultAsync();

            _response.IsSuccess = true;
            _response.Notification = "Lấy dữ liệu thành công";
            _response.Data = userInformationVM;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpPut("ChangeUserPassword")]
    public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordDTO changePasswordDTO)
    {
        try
        {
            ApplicationUser? user = null;

            // Tìm user theo UserId hoặc Email
            if (!string.IsNullOrEmpty(changePasswordDTO.UserId))
            {
                user = await _db.Users.Where(x => x.Id == changePasswordDTO.UserId).FirstOrDefaultAsync();
            }
            else if (!string.IsNullOrEmpty(changePasswordDTO.Email))
            {
                user = await _userManager.FindByEmailAsync(changePasswordDTO.Email);
            }

            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Notification = "Không tìm thấy người dùng";
                _response.Data = null;
                return BadRequest(_response);
            }

            // Kiểm tra mật khẩu cũ
            var checkPassword = await _userManager.CheckPasswordAsync(user, changePasswordDTO.OldPassword);
            
            if (!checkPassword)
            {
                _response.IsSuccess = false;
                _response.Notification = "Mật khẩu cũ không đúng";
                _response.Data = null;
                return BadRequest(_response);
            }

            // Thay đổi mật khẩu
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);
            
            if (result.Succeeded)
            {
                _response.IsSuccess = true;
                _response.Notification = "Đổi mật khẩu thành công";
                _response.Data = "";
                return Ok(_response);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Notification = "Đổi mật khẩu thất bại";
                _response.Data = result.Errors;
                return BadRequest(_response);
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpPut("UpdateUserInformation")]
    public async Task<IActionResult> UpdateUserInformation([FromForm] UpdateUserInformationDTO updateUserInformationDTO)
    {
        try
        {
            // Tìm user theo UserId
            var user = await _db.Users.Where(x => x.Id == updateUserInformationDTO.UserId).FirstOrDefaultAsync();
            
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Notification = "Không tìm thấy người dùng";
                _response.Data = null;
                return BadRequest(_response);
            }

            // Cập nhật thông tin cơ bản
            user.Name = updateUserInformationDTO.Name;
            user.RegionId = updateUserInformationDTO.RegionId;

            // Xử lý upload file Avatar nếu có
            if (updateUserInformationDTO.Avatar != null)
            {
                // Lấy extension của file
                var fileExtension = Path.GetExtension(updateUserInformationDTO.Avatar.FileName);
                
                // Tạo tên file từ UserId và extension
                var fileName = $"{updateUserInformationDTO.UserId}{fileExtension}";
                
                // Tạo đường dẫn đầy đủ để lưu file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars", fileName);
                
                // Tạo thư mục nếu chưa có
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateUserInformationDTO.Avatar.CopyToAsync(stream);
                }

                // Lưu tên file vào database (chỉ tên file, không phải đường dẫn đầy đủ)
                user.Avatar = fileName;
            }

            // Lưu thay đổi vào database
            await _db.SaveChangesAsync();

            _response.IsSuccess = true;
            _response.Notification = "Cập nhật thông tin thành công";
            _response.Data = user;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpDelete("DeleteAccount/{userId}")]
    public async Task<IActionResult> DeleteAccount(string userId)
    {
        try
        {
            // Tìm user theo UserId
            var user = await _db.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Notification = "Không tìm thấy người dùng";
                _response.Data = null;
                return BadRequest(_response);
            }

            // Soft delete: Set IsDeleted = true
            user.IsDeleted = true;
            await _db.SaveChangesAsync();

            _response.IsSuccess = true;
            _response.Notification = "Xóa người dùng thành công";
            _response.Data = user;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO)
    {
        try
        {
            // Tìm user theo email
            var user = await _userManager.FindByEmailAsync(forgotPasswordDTO.Email);
            
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Notification = "Email không tồn tại trong hệ thống";
                _response.Data = null;
                return BadRequest(_response);
            }

            // Tạo OTP (6 chữ số)
            Random random = new();
            string OTP = random.Next(100000, 999999).ToString();
            
            // Lưu OTP vào user
            user.OTP = OTP;
            await _userManager.UpdateAsync(user);
            await _db.SaveChangesAsync();

            // Gửi email chứa OTP
            string subject = "Reset Password Game 106 - " + user.Email;
            string message = "Mã OTP của bạn là: " + OTP;
            await _emailService.SendEmailAsync(user.Email!, subject, message);

            _response.IsSuccess = true;
            _response.Notification = "Gửi mã OTP thành công";
            _response.Data = "email sent to " + user.Email;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpPost("CheckOTP")]
    public async Task<IActionResult> CheckOTP([FromBody] CheckOTPDTO checkOTPDTO)
    {
        try
        {
            // Tìm user theo email
            var user = await _userManager.FindByEmailAsync(checkOTPDTO.Email);
            
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Notification = "Không tìm thấy người dùng";
                _response.Data = null;
                return BadRequest(_response);
            }

            // Convert OTP từ string sang int rồi lại sang string để chuẩn hóa
            var stringOTP = Convert.ToInt32(checkOTPDTO.OTP).ToString();

            // Kiểm tra OTP có đúng không
            if (user.OTP == stringOTP)
            {
                _response.IsSuccess = true;
                _response.Notification = "Mã OTP chính xác";
                _response.Data = user.Email;
                return Ok(_response);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Notification = "Mã OTP không chính xác";
                _response.Data = null;
                return BadRequest(_response);
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
    {
        try
        {
            // Tìm user theo email
            var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Notification = "Không tìm thấy người dùng";
                _response.Data = null;
                return BadRequest(_response);
            }

            // Convert OTP từ string sang int rồi lại sang string để chuẩn hóa
            var stringOTP = Convert.ToInt32(resetPasswordDTO.OTP).ToString();

            // Kiểm tra OTP có đúng không
            if (user.OTP == stringOTP)
            {
                // Đánh dấu OTP đã được sử dụng
                DateTime now = DateTime.Now;
                user.OTP = $"{stringOTP}_used_" + now.ToString("yyyy_MM_dd_HH_mm_ss");

                // Hash password mới
                var passwordHasher = new PasswordHasher<IdentityUser>();
                user.PasswordHash = passwordHasher.HashPassword(user, resetPasswordDTO.NewPassword);

                // Cập nhật user
                var result = await _userManager.UpdateAsync(user);
                
                if (result.Succeeded)
                {
                    _response.IsSuccess = true;
                    _response.Notification = "Đổi mật khẩu thành công";
                    _response.Data = resetPasswordDTO.Email;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Notification = "Đổi mật khẩu thất bại";
                    _response.Data = result.Errors;
                    return BadRequest(_response);
                }
            }
            else
            {
                _response.IsSuccess = false;
                _response.Notification = "Mã OTP không chính xác";
                _response.Data = null;
                return BadRequest(_response);
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Notification = "Lỗi";
            _response.Data = ex.Message;
            return BadRequest(_response);
        }
    }
}

