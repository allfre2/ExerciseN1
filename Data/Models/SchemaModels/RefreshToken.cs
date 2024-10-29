namespace AuthService.Data.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Value { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime Expires { get; set; }
}
