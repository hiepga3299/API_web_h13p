using System.Collections;

namespace API.web_h13p.Application.DTO.UserDTO;

public class UserResponeDTO
{
    public bool isSuccess { get; set; }
    public string Message { get; set; }
    public Object? Data { get; set; } 
}