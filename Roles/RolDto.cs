using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Roles
{
    public class RolDto: IdentityRole
    {
        

        public static implicit operator RolDto(Roles rol)
        {
            RolDto _rol = new RolDto
            {
                Id = rol.Id,
                Name = rol.Name

            };


            return _rol;
        }



    }
}
