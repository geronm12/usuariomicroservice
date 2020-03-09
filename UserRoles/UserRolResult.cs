using MicroServicioUsuarios.Roles;
using MicroServicioUsuarios.Servicios.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.UserRoles
{
    public class UserRolResult: RoleResult
    { 

        public List<Roles.RolDto> Roles { get; set; }

        public UserDto Usuario { get; set; }


    }
}
