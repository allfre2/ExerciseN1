using AuthService.Attributes;
using AuthService.Data.Models;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseN1.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthManager _authManager;
    private readonly ITokenGeneratorService _tokenService;

    public AuthController(IAuthManager authManager, ITokenGeneratorService tokenService)
    {
        _authManager = authManager;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(ClientCredentials credentials)
    {
        if (await _authManager.Authenticate(credentials))
        {
            return Ok(await _tokenService.GenerateAccessToken(credentials));
        }
        else return Unauthorized();
    }

    [HttpGet("verify")]
    [Authorize]
    public async Task<IActionResult> Verify() => Ok();

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshData data)
    {
        var refreshToken = await _tokenService.GetRefreshToken(data);

        if (refreshToken is null) return Unauthorized();

        return Ok(await _tokenService.GenerateAccessToken(refreshToken));
    }

    [HttpPost("register")]
    [RequireApiKey]
    public async Task<IActionResult> Register(RegistrationData data)
    {
        RegisteredClient registration;
        try
        {
            registration = await _authManager.RegisterClient(data);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }

        return Created(HttpContext.Request.Path, registration);
    }
}
