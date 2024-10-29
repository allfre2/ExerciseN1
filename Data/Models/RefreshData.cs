using System.ComponentModel.DataAnnotations;

namespace AuthService.Data.Models;

public class RefreshData
{
    [Required]
    public string ClientId { get; set; }
    [Required]
    public string RefreshToken { get; set; }
}
