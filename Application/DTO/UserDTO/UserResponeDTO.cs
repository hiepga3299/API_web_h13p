using System.Collections;

namespace API.web_h13p.Application.DTO.UserDTO;

public class UserResponeDTO
{
    public bool isSuccess { get; set; }
    public IEnumerable? Message { get; set; }
}