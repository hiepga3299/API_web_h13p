using System.ComponentModel.DataAnnotations;

namespace API.web_h13p.Application.DTO.UserDTO;

public class UserRegisterRequestDTO
{
    [Required(ErrorMessage = "Không được để trống")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Không được để trống")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Không được để trống")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Không được để trống")]
    public string Password { get; set; }
    [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không giống nhau")]
    public string ComfirmPassword { get; set; }
    [Required(ErrorMessage = "Không được để trống")]
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
}