using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.web_h13p.Application.DTO.UserDTO;
using API.web_h13p.Application.Interface;
using API.web_h13p.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.web_h13p.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UserService(UserManager<User> userManager,SignInManager<User> signInManager, IMapper mapper, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<UserResponeDTO> RegisterUserAsync(UserRegisterRequestDTO userRegisterRegister)
    {
        if (userRegisterRegister is null)
        {
            return new UserResponeDTO
            {
                isSuccess = false,
                Message = "Dữ liệu không hợp lệ" 
            };
        }
        var user = _mapper.Map<User>(userRegisterRegister);
        var result = await _userManager.CreateAsync(user, userRegisterRegister.Password);
        if (!result.Succeeded)
        {
            var error = result.Errors.Select(x => x.Description);
            return new UserResponeDTO
            {
                isSuccess = false,
                Message = error.ToString()
            };
        }
        return new UserResponeDTO
        {
            isSuccess = true,
            Message = "Đăng ký thành công"
        };
    }

    public async Task<UserResponeDTO> LoginAsync(UserLoginRequestDTO userLogin)
    {
        if (userLogin is null)
        {
            return new UserResponeDTO
                 { isSuccess = false, Message =  "Dang nhap khong thanh cong" } ;
        }

        var user = await _userManager.FindByNameAsync(userLogin.Username);
        if (user is null)
        {
            return new UserResponeDTO
                { isSuccess = false, Message = "Tai khoan khong ton tai"  } ; 
        }
        var result = await _signInManager.PasswordSignInAsync(user, userLogin.Password, false, false);
        if (!result.Succeeded)
        {
            return new UserResponeDTO
            {
                isSuccess = true,
                Message = "Tên đăng nhâp hoặc mât khẩu không đúng"
            };
        }
        var token =await GenerateJwtToken(userLogin);
        return new UserResponeDTO
        {
            isSuccess = true,
            Message = "Thành công!",
            Data =
            new {
                Token = token,
                Username = user.UserName
            }
        };
    }
    
    public async Task<UserResponeDTO> GetUserAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            return new UserResponeDTO
            {
                isSuccess = false,
                Message = "Tài khoản không tồn tại"
            };
        }
        return new UserResponeDTO
        {
            isSuccess = true,
            Message = "Thành Công" ,
            Data = user.UserName
            
        };
    }

    private async Task<string> GenerateJwtToken(UserLoginRequestDTO userLogin)
    {
        var user = await _userManager.FindByNameAsync(userLogin.Username);
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Name, user.UserName)
        };

        // foreach (var role in roles)
        // {
        //     claim.Add(new Claim(ClaimTypes.Role, role));
        // }

        var credential =
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])),SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(
            audience:_configuration["Jwt:Audience"],
            issuer:_configuration["Jwt:Issuer"],
            claims:claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credential
        );
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}