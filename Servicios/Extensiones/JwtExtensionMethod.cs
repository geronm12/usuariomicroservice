using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Servicios.Extensiones
{
    public static class JwtExtensionMethod
    {
        public static string[] configurations => ConfigExtension.GetConfig(new string[3]);

        public static string GenerateJwtTokens(this Data.Users user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),


                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),

                new Claim(ClaimTypes.NameIdentifier, user.Id)


            };

            //Crear Credenciales Usadas para generar el token

            var credentials = new SigningCredentials(
             new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurations[2])), SecurityAlgorithms.HmacSha256);

            //Generar el JsonWebToken

            var token = new JwtSecurityToken(

                issuer: configurations[0],
                audience: configurations[1],
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: credentials);

            var response = new JwtSecurityTokenHandler().WriteToken(token);
 

             return response;

        }

    }
}
