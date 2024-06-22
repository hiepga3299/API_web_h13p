using System.Runtime.InteropServices.JavaScript;
using API.web_h13p.Application.DTO.UserDTO;
using API.web_h13p.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.web_h13p.Controllers;
[Route("api")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequestDTO userRegisterRequestDto)
    {
        if (userRegisterRequestDto is null)
        {
            return BadRequest("Dữ liệu không hợp lệ");
        }
        var result = await _userService.RegisterUserAsync(userRegisterRequestDto);
        if (!result.isSuccess)
            return BadRequest(result.Message);
        return StatusCode(201, "Đăng ký thành công");
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDTO userLoginRequestDto)
    {
        if (userLoginRequestDto is null)
        {
            return BadRequest("Dữ liệu không hợp lệ");
        }

        var result = await _userService.LoginAsync(userLoginRequestDto);
        if (!result.isSuccess)
        {
            return BadRequest(result.Message);
        }
        Response.Cookies.Append("Token", result.Message.ToString(), new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(10)
        });
        return StatusCode(200, result.Message);
    }
}