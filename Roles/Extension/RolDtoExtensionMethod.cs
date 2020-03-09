using MicroServicioUsuarios.Roles;
using System.Collections.Generic;

namespace MicroServicioUsuarios.UserRoles
{
    public static class RolDtoExtensionMethod
    {
        public static List<RolDto> ConvertToRolDto (this IRolesServicios service, IList<string> roles)
        {
            List<RolDto> _listaRoles = new List<RolDto>();

            foreach (var item in roles)
            {
                var rol = new RolDto();
                rol.Name = item;

                _listaRoles.Add(rol);

            }

            return _listaRoles;
        }

    }
}
