using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VetClinicServer.Models;
using VetClinicServer.Settings;

namespace VetClinicServer.Utils
{
    public class JwtGenerator
    {
        public string GenerateToken(User user)
        {
            List<Claim> claims =
            [
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Role, user.Post.Role.Name)
            ];
            JwtSecurityToken token = new
                (
                    issuer: JwtOptions.ISSUER,
                    audience: JwtOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
