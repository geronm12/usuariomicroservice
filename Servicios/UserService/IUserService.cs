namespace MicroServicioUsuarios.Servicios.UserService
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interfaz que va implementar la clase <see cref="UserService"/>
    /// </summary>
    public interface IUserService
    {
        Task<bool>CrearUserAsync(UserDto dto);


        Task<bool>UpdateUserAsync(UserDto dto);


        Task<bool> LoginAsync(string userName, string password, bool isPersistent);

        Task SignOutAsync();


    }
}
