namespace WebApplication1.ViewModel;

public class RatingVM
{
    public string NameRegion { get; set; } = string.Empty;
    public List<UserResultSum> userResultSums { get; set; } = new List<UserResultSum>();
}

public class UserResultSum
{
    public string NameUser { get; set; } = string.Empty;
    public int SumScore { get; set; }
    public int SumLevel { get; set; }
}





