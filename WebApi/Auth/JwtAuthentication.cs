using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Auth
{
    public class JwtAuthentication
    {

        public JwtAuthentication(UserManager<Person> manager)
        {
            this.manager = manager;
        }

        private const string KEY = "OvoJeNekiStringZaJwtAutentifikaciju";
        private readonly UserManager<Person> manager;

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await manager.FindByNameAsync(username);

            if(user == null) { return null; }

            if(await manager.CheckPasswordAsync(user, password))
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                //usermanager
                //context.Students.Find()....
                claims.Add(new Claim("brojIndeksa", "123/12"));

                var roles = await manager.GetRolesAsync(user);
                claims.Add(new Claim(ClaimTypes.Role, roles.First()));

                ClaimsIdentity identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

                var signingCred = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY)), SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Expires = DateTime.UtcNow.AddHours(1),
                    Subject = identity,
                    SigningCredentials = signingCred
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            return null;
        }
    }
}
