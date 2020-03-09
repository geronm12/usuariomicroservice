using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroServicioUsuarios.UserRoles;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicioUsuarios.Controllers
{
    [Route("api/[controller]")]
    public class UserRoleController : Controller
    {
        private IUserRolesServicios _service;
        public UserRoleController(IUserRolesServicios service)
        {
            _service = service;
        }


        [HttpPost]
        [EnableCors("_myPolicy")]
        [Route("asignar/rol")]
        public async Task<IActionResult> AsignarRol([FromBody]string userNameOrEmail, [FromBody]string roleName)
        {
            var result = await _service.AddToRolAsync(userNameOrEmail, roleName);

            if (result.Succesfull)
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
        [Route("asignar/roles")]
        public async Task<IActionResult> AsignarRoles([FromBody] string userNameOrEmail, [FromBody] List<string> roleName)
        {
            var result = await _service.AddToRolesAsync(userNameOrEmail, roleName);

            if (result.Succesfull)
                return Ok(result);

            return BadRequest(result);


        }


        [HttpGet]
        [EnableCors("_myPolicy")]
        [Route("obtener/usuarios")]
        public async Task<IActionResult> ObtenerPorRoles([FromBody] string roleName)
        {
            var result = await _service.GetUsersInRolAsync(roleName);

            if (result.Succesfull)
                return Ok(result);

            return BadRequest(result);

        }

        [HttpGet]
        [EnableCors("_myPolicy")]
        [Route("obtener/roles/{usernameOrEmail}")]
        public async Task<IActionResult> ObtenerRolesDeUsuario([FromRoute] string usernameOrEmail)
        {
            var result = await _service.GetRolesAsync(usernameOrEmail);

            if (result.Succesfull)
                return Ok(result);

            return BadRequest(result);


        }


        [HttpDelete]
        [EnableCors("_myPolicy")]
        [Route("quitar/rol")]
        public async Task<IActionResult> QuitarRol([FromBody] string usernameOrEmail, [FromBody] string roleName)
        {
            var result = await _service.RemoveFromRolAsync(usernameOrEmail, roleName);


            if (result.Succesfull)
                return Ok(result);

            return BadRequest(result);


        }


        [HttpDelete]
        [EnableCors("_myPolicy")]
        [Route("quitar/roles")]
        public async Task<IActionResult> QuitarRoles([FromBody] string usernameOrEmail, [FromBody] List<string> roles)
        {
            var result = await _service.RemoveFromRolesAsync(usernameOrEmail, roles);


            if (result.Succesfull)
                return Ok(result);


            return BadRequest(result);

            

        }
    }
}