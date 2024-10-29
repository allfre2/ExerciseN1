using AuthService.Attributes;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IAuthManager _authManager;

    public RoleController(IAuthManager authManager)
    {
        _authManager = authManager;
    }

    [HttpGet]
    [RequireApiKey]
    public async Task<IActionResult> ListRoles()
    {
        return Ok(await _authManager.GetRoles());
    }

    [HttpGet("{user}")]
    [RequireApiKey]
    public async Task<IActionResult> ListRoles(string user)
    {
        try
        {
            return Ok(await _authManager.GetRoles(user));
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }

    [HttpPost]
    [RequireApiKey]
    public async Task<IActionResult> AddRole (string role)
    {
        return Created(HttpContext.Request.Path, await _authManager.CreateRole(role));
    }
}
