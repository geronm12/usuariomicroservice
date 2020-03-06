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
    using System;
    using System.Threading.Tasks;
    using System.Web;

    [Route("api/[controller]")]
    public partial class UserController : Controller
    {
        private readonly IConfiguration _config;

        private readonly IUserService _service;

        private delegate string Mensaje(string displayName, string url);

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

                var confirmationUrl = $"{Request.Host.Value}/api/User/verify/email/{Usuario.UID}/{Usuario.EmailCode}";

                var mensaje = new Mensaje(CineAunthenticationMailSender.BodyContentVerificationMail);

                await CineAunthenticationMailSender.SendUserVerificationEmail(Usuario.Name, Usuario.Email, confirmationUrl, mensaje(Usuario.Name, confirmationUrl));


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

        
        [HttpGet]
        [EnableCors("_myPolicy")]
        [Route("password")]
        public async Task<IActionResult> ResetearPasswordMail(string emailOrUserName)
        {
            var result = await _service.ResetPasswordEmail(emailOrUserName);

            if (result.Response)
            {
                var resultado = JsonConvert.DeserializeObject(result.Body);

                var Usuario = JsonConvert.DeserializeObject<ChangePasswordViewModel>(result.Body);

                Usuario.PasswordCode = Usuario.PasswordCode.Replace("/", "%2F").Replace("/", "%2F");

                Usuario.PasswordCode = Usuario.PasswordCode.Replace("+", "%2B").Replace("+", "%2B");

                Usuario.PasswordCode = Usuario.PasswordCode.Replace("=", "%3D").Replace("=", "%3D");

                var confirmationUrl = $"{Request.Host.Value}/api/User/password/reset?email={Usuario.Email}&token={Usuario.PasswordCode}";
 
                var mensaje = new Mensaje(CineAunthenticationMailSender.BodyContentResetPasswordEmail);

                await CineAunthenticationMailSender.SendUserVerificationEmail(Usuario.UserName, Usuario.Email, confirmationUrl, mensaje(Usuario.UserName, confirmationUrl));


                return Ok(new
                {
                    Endpoint = confirmationUrl,
                    Nombre = Usuario.UserName,
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
        [Route("password/reset")]
        public  IActionResult ResetPassword(string email, string token)
        {
            if (email != null || !string.IsNullOrWhiteSpace(email))
                if (token != null || !string.IsNullOrWhiteSpace(token))
                    return Ok(new { Email = email, Token = token });


            return BadRequest();

        }


        [HttpPost]
        [EnableCors("_myPolicy")]
        [Route("password/reset")]
        public async Task<IActionResult> ChangePassword([FromBody]ResetPasswordViewModel model)
        {
            Func<string, string> EsObligatorio = x => $"El campo {x} es obligatorio";


            var error = new Result
            {
                Body = null,
                ErrorMsg = EsObligatorio(string.Empty), 
                Response = false

            };


            if (string.IsNullOrWhiteSpace(model.Mail))
            {
                error.ErrorMsg = EsObligatorio("Mail");

                return BadRequest(error);
            }
            if (string.IsNullOrWhiteSpace(model.Token))
            {
                error.ErrorMsg = EsObligatorio("Token");

                return BadRequest(error);
            }
            if (string.IsNullOrWhiteSpace(model.NewPassword))
            {
                error.ErrorMsg = EsObligatorio("NewPassword");

                return BadRequest(error);
            }
            if (string.IsNullOrWhiteSpace(model.ConfirmNewPassword))
            {
                error.ErrorMsg = EsObligatorio("ConfirmNewPassword");

                return BadRequest(error);
            }
            if (!model.NewPassword.Equals(model.ConfirmNewPassword))
                return BadRequest(new Result { Response = false, Body = null, ErrorMsg = "Los passwords deben coincidir" });




            model.Token = model.Token.Replace("%2F", "/").Replace( "%2F", "/");

            model.Token = model.Token.Replace("%2B", "+").Replace("%2B", "+");

            model.Token = model.Token.Replace("%3D","=").Replace("%3D","=");




            var usuarioAreset = new ResetPasswordDto
            {
                Mail = model.Mail,
                Token = model.Token,
                NewPassword = model.NewPassword,
                ConfirmNewPassword = model.ConfirmNewPassword
            };



            var result = await _service.ResetPassword(usuarioAreset);


            if (result.Response)
                return Ok(result);

            return BadRequest(result);
        }


    }
}