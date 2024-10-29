using System.ComponentModel.DataAnnotations;

namespace AuthService.Data.Models;

public class ClientCredentials
{
    [Required]
    public string ClientId { get; set; }
    [Required]
    public string ClientSecret { get; set; }
}
