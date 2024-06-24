using System.IdentityModel.Tokens.Jwt;

namespace API.web_h13p.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate _next)
    {
        this._next = _next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var token = context.Request.Headers.Authorization.ToString().Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                    if (jwtToken != null)
                    {
                        var user = jwtToken.Claims.First(x => x.Type == "name").Value;
                        if (!string.IsNullOrEmpty(user))
                        {
                            context.Request.Headers.Add("User", user);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Token không hợp lệ");
                }
            }
        }
        await _next(context);
    }
}