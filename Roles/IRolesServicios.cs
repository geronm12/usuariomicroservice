using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Roles
{
    public interface IRolesServicios
    {
        Task<RoleResult> Create(RolDto rol);

        Task<RoleResult> Delete(RolDto rol);

        Task<RoleResult> FindByNameAsync(string roleName);

        Task<RoleResult> FindByIdAsync(string roleId);

        Task<RoleResult> UpdateAsync(RolDto dto);


    }
}
