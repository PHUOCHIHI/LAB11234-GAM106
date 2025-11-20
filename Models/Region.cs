using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication1.Models;

public class Region
{
    public int RegionId { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public Region() { }

    public Region(int id, string name)
    {
        RegionId = id;
        Name = name;
    }
    
    [JsonIgnore]
    public ICollection<User>? Users { get; set; }
}