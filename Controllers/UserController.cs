using API.web_h13p.Application.DTO.UserDTO;
using API.web_h13p.Application.Interface;
using API.web_h13p.Controllers.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.web_h13p.Controllers;

[Route("/api")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet("checkuser")]
    public async Task<IActionResult> CheckUser()
    {
        var user = HttpContext.Request.Headers["User"].ToString();
        var token = HttpContext.Request.Headers.Authorization.ToString().Split(" ").Last();
        if (string.IsNullOrEmpty(user))
        {
            return BadRequest("User không hợp lệ");
        }

        var result = await _userService.GetUserAsync(user);
        if (!result.isSuccess)
        {
            return BadRequest("User không hợp lệ");
        }
        return Ok(new ResponseDTO
        {
            Message = "Thanh Cong!",
            Data = new
            {
                User = result.Data,
                Token = token
            }
        });
    }
}