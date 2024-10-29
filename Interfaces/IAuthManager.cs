using AuthService.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Interfaces;

public interface IAuthManager
{
    public Task<bool> Authenticate(ClientCredentials credentials);
    public Task<RegisteredClient> RegisterClient(RegistrationData data);
    public Task<IEnumerable<IdentityRole>> GetRoles();
    public Task<IEnumerable<string>> GetRoles(string userName);
    public Task<IdentityRole> CreateRole(string name);
}
