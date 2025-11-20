using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [ForeignKey("Region")]
    public int RegionId { get; set; }
    
    public string? Avatar { get; set; }
    
    public bool IsDeleted { get; set; } = false;
    
    public string? OTP { get; set; }
    
    public DateTime? OTPExpiry { get; set; }
}

