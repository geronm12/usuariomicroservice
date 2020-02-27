namespace MicroServicioUsuarios.Servicios.UserService
{
    using global::AutoMapper;
    using MicroServicioUsuarios.Data;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using static MicroServicioUsuarios.AutoMapper.AutoMapperService;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Clase que implementa la interfaz <see cref="IUserService"/>
    /// </summary>

    public class UserService : IUserService
    {
        #region Variables Inyectadas

        /// <summary>
        /// Instancia de <see cref="ApplicationContext"/>(Scooped)
        /// </summary>
        protected ApplicationContext _context;

        /// <summary>
        /// Administra/Gestiona los usuarios (crear,eliminar, buscar, roles, etc)
        /// </summary>

        protected UserManager<Users> _userManager;

        /// <summary>
        /// Administra el inicio de sesión de los usuarios
        /// </summary>

        protected SignInManager<Users> _userSignIn;

        protected HttpContext HttpContext;

        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public UserService(ApplicationContext context, UserManager<Users> userManager, SignInManager<Users> userSignIn,
            HttpContext httpContext)
        {
            _context = context;

            _userManager = userManager;

            _userSignIn = userSignIn;

            _mapper = CrearMapa();

            HttpContext = httpContext;

        }
        #endregion

        #region Crear Usuario 
        /// <summary>
        /// Servicio para crear un usuario usando el UserManager
        /// </summary>
        /// <param name="dto">Se pide como parámetro <see cref="UserDto"/> y luego se Mappea a la entidad <see cref="Users"/></param>
        /// <returns></returns>

        public async Task<bool> CrearUserAsync(UserDto dto)
        {
            Users newUser = _mapper.Map<Users>(dto);

           var result =  await _userManager.CreateAsync(newUser);

            if (result.Succeeded)
                return result.Succeeded;

            return false;

        }

        #endregion

        #region ModificarUsuario
        /// <summary>
        /// De la misma forma que el <see cref="Crear Usuario"/> Update modifica los datos pasados por parámetro.
        /// </summary>
        /// <param name="dto">Se pide como parámetro <see cref="UserDto"/> y luego se mapea  a la entidad <see cref="Users"/></param>
        /// <returns></returns>

        public async Task<bool> UpdateUserAsync(UserDto dto)
        {
            var foundUser = await _userManager.FindByNameAsync(dto.UserName);

            if (foundUser != null)
            {
                foundUser = _mapper.Map<Users>(dto);
                var result = await _userManager.UpdateAsync(foundUser);
                if (result.Succeeded)
                    return result.Succeeded;

                return false;
            }
            
            return false;
        }

        #endregion

        #region Login/ Inicio de Sesion
        /// <summary>
        /// - Se utiliza SignIn de identity para manejar el inicio de sesión del Usuario
        /// - Previo a realizar el inicio de sesión se cierra la sesión anterior si es que hubiera.
        /// </summary>
        /// <param name="userName">Nombre de Usuario</param>
        /// <param name="password">Contraseña del Usuario</param>
        /// <param name="isPersistent">Si el usuario desea que se guarde su password en los cookies para no tener que volver
        /// a inicar sesión de nuevo.</param>
        /// <parm name="lockOutOnFailure">Booleano que indica si se debe bloquear el usuario a "X" cantidad de intentos fallidos.
        /// Normalmente son 3.</parm>
        /// <returns>Devuelve un booleano que indica si ingreso es correcto o no</returns>
        public async Task<bool> LoginAsync(string userName, string password, bool isPersistent)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            var result = await _userSignIn.PasswordSignInAsync(userName, password, isPersistent, true);

            if (result.Succeeded)
                return result.Succeeded;


            return result.Succeeded;

        }
        #endregion

        #region Cerrar Sesión
        //Método para cerrar la sesión.
        public async Task SignOutAsync()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        }

        #endregion
    }
}
