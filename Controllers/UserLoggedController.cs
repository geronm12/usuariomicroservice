using MicroServicioUsuarios.Servicios.Extensiones;
using MicroServicioUsuarios.Servicios.UserService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Controllers
{
    [AuthorizeToken]
    [Route("api/[controller]")]
    public partial class UserLoggedController: Controller
    {
        private readonly IUserService _service;

        public UserLoggedController(IUserService service)
        {
            _service = service;

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



    }
}
