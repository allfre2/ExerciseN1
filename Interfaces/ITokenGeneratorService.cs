using AuthService.Data.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Interfaces;

public interface ITokenGeneratorService
{
    public Task<AccessToken> GenerateAccessToken(ClientCredentials credentials);
    public Task<AccessToken> GenerateAccessToken(RefreshToken refreshToken);
    public Task<string> GenerateToken(ClientCredentials credentials);
    public Task<RefreshToken?> GetRefreshToken(RefreshData data);
    public Task<RefreshToken> GenerateRefreshToken(ClientCredentials credentials);
    public SecurityToken ParseToken(string token);
}
