
using Microsoft.Extensions.Configuration;

namespace MicroServicioUsuarios.Servicios.Extensiones
{
    public class ConfigExtension
    {
        public static IConfiguration _config;

        public ConfigExtension(IConfiguration config)
        {
            _config = config;
        }

        public static string[] GetConfig(string[] array)
        {
            array[0] = _config["Jwt:Issuer"];
            array[1] = _config["Jwt:Audience"];
            array[2] = _config["Jwt:SecretKey"];

            return array;
        }

    }
}
