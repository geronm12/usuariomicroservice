using AutoMapper;
using Mailer;
using Mailer.CineEmailSender;
using Mailer.Templates;
using MicroServicioUsuarios.Data;
using MicroServicioUsuarios.Servicios.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.IoC
{
    public static class Ioc
    {
        /// <summary>
        /// Clase estática que recibe como parámetros IService Collection desde <see cref="Startup"/>
        /// Acá se inyectan todos los servicios desde este contenedor.  
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>

        #region Metodo de configuratión

        public static void ConfigurationServices(IServiceCollection services, IConfiguration configuration)
        {
            //Inyección del DataContext
            services.AddDbContext<ApplicationContext>();

            //Inyección de las configuraciones para poder utilizar el appsettings.json desde cualquier otra clases/proyecto relacionado con la API
            services.AddTransient<IConfiguration>(provider => configuration);

            //Agregamos el session para almacenar el token en HttpContext
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromHours(5);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;

            });



            //Agrega Identity, agrega authenticación basada en cookies
            //Inyecta servicios de tipo scooped a clases como UserManager, SinginManager, PasswordHashers, etc...
            //NOTA: Agrega automáticamente el usuario validado desde los cookies al HttpContext.User

            services.AddIdentity<Users, IdentityRole>()
                //Agrega el almacenamiento de usuarios y los roles de este dbcontext
                //que son utilizados por el UserManager y el RoleManager
                .AddEntityFrameworkStores<ApplicationContext>()
                //Agrega el generador de tokens, llaves únicas, hashes,
                //para cosas como links de recuperación de passwords, verificación por n° de teléfono, etc...
                .AddDefaultTokenProviders();

            
            #region Authenticacion Mediante JsonWebToken
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
                };

            });
            #endregion


            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IEmailSender, SendGridEmailSender>();

            services.AddTransient<IEmailTemplateSender,EmailTemplateSender>();

             
        }

        #endregion
    }

}
