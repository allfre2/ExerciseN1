namespace AuthService.Data.Models;

public class ApiKey
{
    public int Id { get; set; }
    public string Key { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; }
}
