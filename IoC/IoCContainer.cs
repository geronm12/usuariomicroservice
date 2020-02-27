using AutoMapper;
using MicroServicioUsuarios.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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

            services.AddHttpContextAccessor();

        }

        #endregion
    }

}
