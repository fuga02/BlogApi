using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlogApi.Entities;
using BlogApi.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Managers.Identity;

public class JwtTokenManager
{
    private JwtOptions _jwtOptions;

    public JwtTokenManager(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var signingKey = System.Text.Encoding.UTF32.GetBytes(_jwtOptions.SigningKey);
        var security = new JwtSecurityToken(issuer: _jwtOptions.ValidIssuer,
            audience: _jwtOptions.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtOptions.ExpiresInMinutes),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256));

        var token = new JwtSecurityTokenHandler().WriteToken(security);
        return token;
    }

}