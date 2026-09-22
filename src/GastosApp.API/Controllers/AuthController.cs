using GastosApp.Application.DTOs;
using GastosApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GastosApp.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var resultado = await _authService.RegistrarAsync(request);
        return Ok(resultado);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var resultado = await _authService.LoginAsync(request);
        return Ok(resultado);
    }
}
