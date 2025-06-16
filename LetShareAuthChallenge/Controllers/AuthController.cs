using LetShareAuthChallenge.Dtos;
using LetShareAuthChallenge.Services;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        try
        {
            var (access, refresh) = await _authService.AuthenticateAsync(
                request.Username,
                request.Password
            );

            return Ok(new
            {
                access_token = access,
                refresh_token = refresh
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}