using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Photosnap_API.Jwt
{
    public class JwtToken
    {
        public static string GenerateToken(string username)
        {
            var listOfclaims = new List<Claim>();
            listOfclaims.Add(new Claim(ClaimTypes.Name, username));
            listOfclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            listOfclaims.Add(new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(
                                                                    DateTime.Now.AddHours(3)).ToUnixTimeSeconds().ToString()));
            var claims = listOfclaims.ToArray();
            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Lorem ipsum dolor sit amet, consectetur adipiscing elit")),
                                             SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            var handler = new JwtSecurityTokenHandler();
            string tokenString = handler.WriteToken(token);
            return tokenString;
        }
    }
}
