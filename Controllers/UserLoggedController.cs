using MicroServicioUsuarios.Servicios.Extensiones;
using MicroServicioUsuarios.Servicios.UserService;
using MicroServicioUsuarios.ViewModels;
using MicroServicioUsuarios.Whatsapp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace MicroServicioUsuarios.Controllers
{
    [AuthorizeToken]
    [Route("api/[controller]")]
    public partial class UserLoggedController: Controller
    {
        private readonly IUserService _service;

        private readonly IConfiguration _config;

        private readonly IWhatsappSender _sender;
        public UserLoggedController(IUserService service, IConfiguration config, IWhatsappSender sender)
        {
            _service = service;

            _config = config;

            _sender = sender;

        }

        [HttpGet]
        [AuthorizeToken]
        [Route("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            if(HttpContext.User != null)
            {

                 var usuario = await _service.GetUserProfile(HttpContext.User);

                if (usuario != null)
                    return Ok(JsonConvert.SerializeObject(usuario));

                return BadRequest();

            }



            return Ok();
        }


        [HttpPost]
        [Route("whatsapp")]
        [AllowAnonymous]
        public async Task<IActionResult> SendWhatsapp(WhatsappMessageViewModel model, string email)
        {
             

            var mensaje = new WhatsappSenderDto();
            mensaje.CodPais = model.CodigoDePais;

            mensaje.CodCiudad = model.CodigoDeCiudad;
            mensaje.NumeroCelular = model.NumCel;
            mensaje.Verificacion = Whatsapp.ExtensionesYHelpers.FormaVerificacion.Whatsapp;

            var result = await _sender.EnviarMensaje(mensaje,Whatsapp.ExtensionesYHelpers.FormaVerificacion.Whatsapp,email);

            if (result.Succesful)
                return Ok(result);

            return BadRequest();
        }

        [HttpPost]
        [Route("whatsapp/verify")]
        [EnableCors("_myPolicy")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifiCode(string codigo, string usernameOrEmail)
        {
             
            var response = await _sender.VerificarCódigo(new WhastappUserVerificationDto {NombreUsuarioOEmail = usernameOrEmail, Codigo = codigo});

            if (response.Succesful)
                return Ok(response);


            return BadRequest();

        }

    }
}
