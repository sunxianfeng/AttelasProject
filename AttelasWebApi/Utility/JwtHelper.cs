using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Attelas.Utility;

public static class JwtHelper
{
    public static string CreateToken(string username)
    {
        // 1. Define the Claims that need to be used
        var claims = new[]
        {
            new Claim(ClaimTypes.Role, "admin"), //HttpContext.User.IsInRole("r_admin")
            new Claim(ClaimTypes.Name, username),
        };
        // 2. Read SecretKey from appsettings.json
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetConfigurations.GetConfiguration("Jwt:SecretKey")));
        // 3. Choose the encryption algorithm
        var algorithm = SecurityAlgorithms.HmacSha256;
        // 4. Generate credentials
        var signingCredentials = new SigningCredentials(secretKey, algorithm);
        // 5. Generate token
        var jwtSecurityToken = new JwtSecurityToken(
            GetConfigurations.GetConfiguration("Jwt:Issuer"),           //Issuer
            GetConfigurations.GetConfiguration("Jwt:Audience"),      //Audience
            claims,                                      //Claims,
            DateTime.Now,                       //notBefore
            DateTime.Now.AddSeconds(300),        //expires
            signingCredentials                         //Credentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return token;
    }
}