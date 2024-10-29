using System.ComponentModel.DataAnnotations;

namespace AuthService.Data.Models;

public class RegistrationData
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public IEnumerable<string> Roles { get; set; }
    public IDictionary<string, string>? Claims { get; set; }
}
