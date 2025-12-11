using Microsoft.AspNetCore.Http;

namespace WebApplication1.DTO;

public class UpdateUserInformationDTO
{
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int RegionId { get; set; }
    public IFormFile? Avatar { get; set; }
}















