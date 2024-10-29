using AuthService.Data;
using AuthService.Data.Models;
using AuthService.Exceptions;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthService.Implementation;

public class AuthManager : IAuthManager
{
    private readonly AuthContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthManager(
        AuthContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<bool> Authenticate(ClientCredentials credentials)
    {
        var identityUser = await _userManager.FindByNameAsync(credentials.ClientId);

        if (identityUser is null) return false;

        return await _userManager.CheckPasswordAsync(identityUser, credentials.ClientSecret);
    }

    public async Task<RegisteredClient> RegisterClient(RegistrationData data)
    {
        var roles = await ValidateRoles(data.Roles);
        
        var clientId = data.UserName;
        var clientSecret = data.Password;

        var identityUser = new IdentityUser
        {
            UserName = clientId,
            Email = data.Email,
        };

        var result = await _userManager.CreateAsync(identityUser, clientSecret);

        if (!result.Succeeded) throw new RegistrationException(result.ToString());

        var claimsResult = await _userManager.AddClaimsAsync(
            identityUser,
            data.Claims?.Select(pair => new Claim(pair.Key, pair.Value)) ?? new List<Claim>()
        );

        if (!claimsResult.Succeeded) throw new ClaimsAdditionException(result.ToString());

        var rolesResult = await _userManager.AddToRolesAsync(identityUser, roles);

        if (!rolesResult.Succeeded) throw new RoleAdditionException(result.ToString());

        await _context.SaveChangesAsync();

        return new RegisteredClient
        {
            Email = data.Email,
            ClientId = identityUser.UserName,
            ClientSecret = clientSecret,
            Roles = roles,
            Claims = data.Claims,
            CreatedOn = DateTime.Now
        };
    }

    public async Task<IEnumerable<IdentityRole>> GetRoles() => _roleManager.Roles;

    public async Task<IEnumerable<string>> GetRoles(string userName)
    {
        var identityUser = await _userManager.FindByNameAsync(userName);
        var roles = await _userManager.GetRolesAsync(identityUser);

        return roles;
    }

    public async Task<IdentityRole> CreateRole(string name)
    {
        var result = await _roleManager.CreateAsync(new IdentityRole { Name = name });

        if (!result.Succeeded) throw new RoleAdditionException(result.ToString());

        return await _roleManager.FindByNameAsync(name);
    }

    #region Private Methods

    private async Task<IEnumerable<string>> ValidateRoles(IEnumerable<string> roles)
    {
        foreach (var role in roles)
        {
            if (await _roleManager.FindByNameAsync(role) is null)
            {
                throw new RoleNotFoundException($"El rol: {role} no existe.");
            }
        }

        return roles;
    }

    #endregion
}
