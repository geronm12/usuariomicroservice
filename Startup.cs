
namespace MicroServicioUsuarios
{
    using Mailer.CineEmailSender;
    using MicroServicioUsuarios.Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using System;
    using static IoC.Ioc;

    public class Startup
    {

        public static string _myPolicy => "_myPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>();

            services.AddControllersWithViews();

            //Configuramos las opciones para el Identity: puede ser para passwords, tokens, etc...

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            });

            //Configuramos las opciones para los cookies como tiempo de expiración, ruta para el login, etc.

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "api/User/login";

                options.ExpireTimeSpan = TimeSpan.FromSeconds(15);
            });




            #region SwaggerGen
            ///<summary>
            /// Agregamos SwaggerGen a nuestra WebApi Restfull.  
            /// </summary>
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "User M.S",
                    Version = "v1",
                    Description = "Micro Servicio para gestionar y administrar usuarios utilizando EF Core 3.1"

                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });


                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                           new OpenApiSecurityScheme
                           {
                              Reference = new OpenApiReference
                              {
                                     Type = ReferenceType.SecurityScheme,
                                     Id = "Bearer"
                              }
                           },
                           new string[] { }
                    }
                });
            });
            #endregion


            //Agregamos MVC a los servicios, aquí establecemos la compatibilidad con la versión del Framework y en las opciones 
            //establecemos el EndPointRouting en FALSE para poder usar mvc en Configure.

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                    .AddMvcOptions(options => options.EnableEndpointRouting = false);


                //Llamamos a la clase estática para inyectar desde un archivo externo todos los servicios.
                ConfigurationServices(services, Configuration);

            #region Cambiar
            #warning CAMBIAR
            ConfigExtension ext = new ConfigExtension(Configuration);

            Servicios.Extensiones.ConfigExtension ext2 = new Servicios.Extensiones.ConfigExtension(Configuration);

            #endregion


            //Configuramos el cors para que se pueda utilizar la API desde cualquier origen (No recomendable para deploy
            //Si para produccion
            #warning CAMBIAR ANTES DEL DEPLOY

            services.AddCors(options => options.AddPolicy(_myPolicy, options =>
                options.WithOrigins("*").WithMethods("*").WithHeaders("*")));


            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
         
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseSession();


            //Establecemos el uso del MVC y ponemos en las opciones el MapRoute por defecto.

            app.UseMvc(options =>
            {
                options.MapRoute(name: "default", template: "{controller-home}/{action=Index}/{Id}");
            });

            //Establecemos el uso de Swagger.

            app.UseSwagger();

            //Indicamos el uso de swagger UI.

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "UserMs");
            });

            app.UseCors(_myPolicy);


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
