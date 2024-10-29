namespace AuthService.Data.Models;

public class RegisteredClient
{
    public string Email { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public IDictionary<string,string>? Claims { get; set; }
    public DateTime CreatedOn { get; set; }
}
