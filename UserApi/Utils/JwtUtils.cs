using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserApi.Models;

namespace UserApi.Utils
{
    public class JwtUtils
    {
        public static string GenerateJwtToken(User user, string key, string issuer, string audience, string role)
        {

            var claim = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                new Claim(ClaimTypes.NameIdentifier, user.Id),

                new Claim(ClaimTypes.Role, role)
            };
            /*
            foreach (var role in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role, role));
            }
            */
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(

                issuer: issuer,
                audience: audience,
                claims: claim,
                signingCredentials: creds,
                expires: DateTime.Now.AddDays(1)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
