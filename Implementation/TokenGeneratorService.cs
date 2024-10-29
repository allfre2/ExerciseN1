using AuthService.Configuration;
using AuthService.Data;
using AuthService.Data.Models;
using AuthService.Exceptions;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Implementation;

public class TokenGeneratorService : ITokenGeneratorService
{
    private readonly JwtConfiguration _jwtConfig;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AuthContext _authContext;

    public TokenGeneratorService(
        IOptions<JwtConfiguration> jwtConfig,
        UserManager<IdentityUser> userManager,
        AuthContext authContext
    )
    {
        _jwtConfig = jwtConfig.Value;
        _userManager = userManager;
        _authContext = authContext;
    }

    public async Task<AccessToken> GenerateAccessToken(ClientCredentials credentials)
    {
        var accessToken = new AccessToken
        {
            Token = await GenerateToken(credentials),
            RefreshToken = (await GenerateRefreshToken(credentials)).Value,
            Type = "Bearer",
            Expires = TimeSpan.FromMinutes(_jwtConfig.TokenExpirationMinutes)
        };

        await _authContext.SaveChangesAsync();

        return accessToken;
    }

    public async Task<AccessToken> GenerateAccessToken(RefreshToken refreshToken)
    {
        var accessToken = new AccessToken
        {
            Token = await GenerateToken(new ClientCredentials { ClientId = refreshToken.UserName }),
            RefreshToken = (await GenerateRefreshToken(new ClientCredentials { ClientId = refreshToken.UserName })).Value,
            Type = "Bearer",
            Expires = TimeSpan.FromMinutes(_jwtConfig.TokenExpirationMinutes)
        };

        await _authContext.SaveChangesAsync();
        
        return accessToken;
    }

    public async Task<string> GenerateToken(ClientCredentials credentials)
    {
        var identityUser = await _userManager.FindByNameAsync(credentials.ClientId);
        var userClaims = await _userManager.GetClaimsAsync(identityUser);
        var roles = await _userManager.GetRolesAsync(identityUser);

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, identityUser.UserName) };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        claims.AddRange(userClaims);

        var token = new JwtSecurityTokenHandler()
            .WriteToken(new JwtSecurityToken(
                claims: claims,
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                expires: DateTime.Now.AddMinutes(_jwtConfig.TokenExpirationMinutes),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret)),
                    SecurityAlgorithms.HmacSha512Signature
                )
            ));

        return token;
    }

    public async Task<RefreshToken?> GetRefreshToken(RefreshData data)
    {
        return await _authContext.RefreshTokens.SingleOrDefaultAsync(t => 
            t.Value == data.RefreshToken 
            && t.UserName == data.ClientId 
            && DateTime.Now < t.Expires
        );
    }

    public async Task<RefreshToken> GenerateRefreshToken(ClientCredentials credentials)
    {
        // Remove all refreshTokens of the user to prevent reuse
        _authContext.RefreshTokens
            .RemoveRange(_authContext.RefreshTokens.Where(token => token.UserName == credentials.ClientId));

        var token = new RefreshToken
        {
            UserName = credentials.ClientId,
            Value = GetRefreshTokenString(),
            Expires = DateTime.Now.AddMinutes(_jwtConfig.TokenExpirationMinutes * 3)
        };

        await _authContext.RefreshTokens.AddAsync(token);        
        
        return token;
    }

    #region Helper Methods

    public SecurityToken ParseToken(string token)
    {
        return new JwtSecurityTokenHandler().ReadJwtToken(token);
    }

    private static string GetRefreshTokenString() => Guid.NewGuid().ToString("N");

    #endregion
}
