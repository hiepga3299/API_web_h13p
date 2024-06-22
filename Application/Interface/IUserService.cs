using API.web_h13p.Application.DTO.UserDTO;

namespace API.web_h13p.Application.Interface;

public interface IUserService
{
    Task<UserResponeDTO> RegisterUserAsync(UserRegisterRequestDTO userRegisterRegister);
    Task<UserResponeDTO> LoginAsync(UserLoginRequestDTO userLogin);
}