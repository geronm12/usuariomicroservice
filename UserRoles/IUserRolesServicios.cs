using MicroServicioUsuarios.Servicios.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.UserRoles
{
    public interface IUserRolesServicios
    {
        Task<UserRolResult> AddToRolAsync(string userNameOrEmail, string roleName);

        Task<UserRolResult> AddToRolesAsync(string userNameOrEmail, List<string> roles);

        Task<UserRolResult> GetRolesAsync(string userNameOrEmail);

        Task<UserRolResult> GetUsersInRolAsync(string roleName);

        Task<UserRolResult> RemoveFromRolAsync(string userNameOrEmail, string roleName);

        Task<UserRolResult> RemoveFromRolesAsync(string userNameOrEmail, List<string> roles);


    }
}
