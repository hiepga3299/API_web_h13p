using Microsoft.AspNetCore.Identity;

namespace API.web_h13p.Domain.Entities;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}