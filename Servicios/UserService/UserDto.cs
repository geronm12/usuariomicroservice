using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Servicios.UserService
{
    /// <summary>
    /// Dto creado para no llegar con la entidad Users al frontend.
    /// </summary>

    public class UserDto: IdentityUser
    {
    }
}
