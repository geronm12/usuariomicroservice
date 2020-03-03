namespace MicroServicioUsuarios.Controllers
{
    using Mailer.CineEmailSender;
    using MicroServicioUsuarios.Servicios.UserService;
    using MicroServicioUsuarios.ViewModels;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

   

    [Route("api/[controller]")]
    public partial class UserController : Controller
    {
        private readonly IConfiguration _config;

        private readonly IUserService _service;
        public UserController(IConfiguration config, IUserService service)
        {

            _config = config;

            _service = service;

        }
 

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            var loginModel = new LoginDto
            {
                UsuarioOEmail = model.UsuarioOEmail,
                Password = model.Password
            };


            var result = await _service.LoginAsync(loginModel);

           

            if (result.Response)
            {

                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            
            }

             
            
            
        }
       
        [HttpPost]
        [EnableCors("_myPolicy")]
        [Route("crear")]
        public async Task<IActionResult> Crear([FromBody] UsuarioCreacionViewModel model)
        {
            var userModel = new UserDto
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };


           var result = await _service.CrearUserAsync(userModel, model.Password);
           
            if(result.Response)
            {
                var resultado = JsonConvert.DeserializeObject(result.Body);

                
                var Usuario = JsonConvert.DeserializeObject<UserResultVerificationMail>(result.Body);

                Usuario.EmailCode = Usuario.EmailCode.Replace("/", "%2F").Replace("/", "%2F");

                Usuario.EmailCode = Usuario.EmailCode.Replace("+", "%2B").Replace("+", "%2B");

                Usuario.EmailCode = Usuario.EmailCode.Replace("=", "%3D").Replace("=", "%3D");

                var confirmationUrl = $"https://localhost:5001/api/User/verify/email/{Usuario.UID}/{Usuario.EmailCode}";

                await CineAunthenticationMailSender.SendUserVerificationEmail(Usuario.Name, Usuario.Email, confirmationUrl);


                return Ok(new
                {
                  Endpoint = confirmationUrl,
                  Nombre = Usuario.Name,
                  Mail = Usuario.Email
                });

            }
            else
            {
                return BadRequest(result);
            }

            
        }

        [HttpGet]
        [EnableCors("_myPolicy")]
        [Route("verify/email/{userId}/{emailToken}")]
        public async Task<IActionResult> VerificarEmail(string userId, string emailToken)
        {
            var result = await _service.VerifyEmailAsync(userId, emailToken);

            if(result.Response)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        
    }
}