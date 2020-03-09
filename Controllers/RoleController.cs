using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroServicioUsuarios.ResModels;
using MicroServicioUsuarios.Roles;
using MicroServicioUsuarios.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MicroServicioUsuarios.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IRolesServicios _services;

        public RoleController(IRolesServicios services)
        {
            _services = services;
        }

        [HttpPost]
        [Route("crear")]
        [EnableCors("_myPolicy")]
        public async Task<IActionResult> CrearRol(RolViewModel rol)
        {
            RolDto _rol = new RolDto
            {
                Name = rol.Name
            };

            var rest = await _services.Create(_rol);

            if (rest.Succesfull)
            {
                HttpJsonResModel<RolDto> res = JsonConvert.DeserializeObject<HttpJsonResModel<RolDto>>(rest.Body);
                res.Objeto = _rol;

                return Ok(res);

            }

            return BadRequest(rest);



        }

        [HttpGet]
        [Route("get/{roleId}")]
        [EnableCors("_myPolicy")]
        public async Task<IActionResult> ObtenerPorId(string roleId)
        {
            var res = await _services.FindByIdAsync(roleId);

            if(res.Succesfull)
            {
                return Ok(res);

            }

            return BadRequest(res);



        }

        [HttpGet]
        [Route("get/{roleName}")]
        [EnableCors("_myPolicy")]
        public async Task<IActionResult> ObtenerPorNombre(string roleName)
        {
            var res = await _services.FindByNameAsync(roleName);

            if (res.Succesfull)
            {
                return Ok(res);

            }

            return BadRequest(res);


        }


        [HttpDelete]
        [Route("delete")]
        [EnableCors("_myPolicy")]
        public async Task<IActionResult> EliminarRol(RolViewModel rol)
        {
            RolDto _rol = new RolDto
            {
                Id = rol.Id,
                Name = rol.Name
            };

            var request = await _services.Delete(_rol);

            if(request.Succesfull)
            {
                HttpJsonResModel<RolDto> res = JsonConvert.DeserializeObject<HttpJsonResModel<RolDto>>(request.Body);
                res.Succesfull = true;
                res.Objeto = _rol;
                return Ok(res);
            }

            return BadRequest(request);

        }

        [HttpPut]
        [Route("update")]
        [EnableCors("_myPolicy")]
        public async Task<IActionResult> ModificarRol(RolViewModel rol)
        {
            RolDto _rol = new RolDto
            {
                Id = rol.Id,
                Name = rol.Name
            };

            var request = await _services.UpdateAsync(_rol);

            if (request.Succesfull)
            {
                HttpJsonResModel<RolDto> res = JsonConvert.DeserializeObject<HttpJsonResModel<RolDto>>(request.Body);
                res.Succesfull = true;
                res.Objeto = _rol;
                return Ok(res);
            }

            return BadRequest(request);


        }

    }
}