

namespace MicroServicioUsuarios.Data
{
    using MicroServicioUsuarios.Data.MetaData;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Clase donde se configura la conexión con la base de datos
    /// </summary>
    public class ApplicationContext : IdentityDbContext<Users>
    {
 
        private readonly IConfiguration _config;

        #region Constructor

        public ApplicationContext(IConfiguration config)
        {
            _config = config;

        }
        #endregion



        /// <summary>
        ///  Sobreescritura del método <see cref="OnConfiguring(DbContextOptionsBuilder)"/> que sirve para configurar el DbContext.
        /// </summary>
        /// <param name="builder">Opciones para configurar el DbContext
        /// Pasamos como parámetro el AppSettingsJson dónde se encuentra el ConnectionStrings
        /// Normalmente se puede realizar desde una clase externa configurando la cadena de conexión pero a la hora de levantar 
        /// el contenedor en docker o de realizar un push en github estaríamos enviando los datos de nuestra connection strings.
        /// Desde el appsettings.json se puede encriptar.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {

             builder.UseMySql(_config["ConnectionString:DefaultConnection"]);
          
        }

        /// <summary>
        /// Sobreescritura del método <see cref="OnModelCreating(ModelBuilder)"/> que sirve para configurar el modelo E-R de la base 
        /// de datos. (APIFLUENT-DATAANOTTATIONS, etc)
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
             
            base.OnModelCreating(builder);
        }

       
    }
}

