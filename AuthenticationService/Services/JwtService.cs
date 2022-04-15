using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Services{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration=configuration;
        }
        public string IssueToken(ReadUserDto readUserDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _configuration.GetSection("JWTKey").Value;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]{new Claim("id",readUserDto.Id),new Claim("username",readUserDto.Username)}),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),SecurityAlgorithms.HmacSha256Signature)
            };
            var token=tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GetUserIdFromToken(string token){
            var jwt=new JwtSecurityToken(token.Substring(7));
            var id=jwt.Claims.First(c=>c.Type=="id").Value;

            return id;
        }
    }
}