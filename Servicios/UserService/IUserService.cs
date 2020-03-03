﻿namespace MicroServicioUsuarios.Servicios.UserService
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    /// Interfaz que va implementar la clase <see cref="UserService"/>
    /// </summary>
    public interface IUserService
    {
        Task<Result>CrearUserAsync(UserDto dto, string password);


        Task<Result>UpdateUserAsync(UserDto dto, string userLogged);


        Task<Result> LoginAsync(LoginDto credentials);

        Task<Result> VerifyEmailAsync(string userId, string emailToken);
 

        Task<Result> GetUserProfile(ClaimsPrincipal userName);


    }
}
