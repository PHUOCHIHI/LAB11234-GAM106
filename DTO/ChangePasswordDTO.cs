namespace WebApplication1.DTO;

public class ChangePasswordDTO
{
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

