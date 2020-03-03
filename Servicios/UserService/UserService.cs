namespace MicroServicioUsuarios.Servicios.UserService
{
    using global::AutoMapper;
    using MicroServicioUsuarios.Data;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using static MicroServicioUsuarios.AutoMapper.AutoMapperService;
    using System.Web;
    using Mailer;
    using Mailer.CineEmailSender;
    using Newtonsoft.Json;
    using MicroServicioUsuarios.Servicios.Extensiones;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

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

        //protected Microsoft.AspNetCore.Http.HttpContext HttpContext;

        private readonly IMapper _mapper;

        private readonly IEmailSender _sender;

        #endregion

        #region Constructor
        public UserService(ApplicationContext context, UserManager<Users> userManager, SignInManager<Users> userSignIn,
              IEmailSender sender)
        {
            _context = context;

            _userManager = userManager;

            _userSignIn = userSignIn;

            _mapper = CrearMapa();

            
            _sender = sender;

        }
        #endregion

        #region Crear Usuario 
        /// <summary>
        /// Servicio para crear un usuario usando el UserManager
        /// </summary>
        /// <param name="dto">Se pide como parámetro <see cref="UserDto"/> y luego se Mappea a la entidad <see cref="Users"/></param>
        /// <returns></returns>

        public async Task<Result> CrearUserAsync(UserDto dto, string password)
        {
            Users newUser = _mapper.Map<Users>(dto);

            var result =  await _userManager.CreateAsync(newUser, password);

            if (result.Succeeded)
            {

                var userIdentity = await _userManager.FindByNameAsync(dto.UserName);

                var emailVerificationCode = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                var Url = "localhost:5001/swagger";

                var confirmationUrl = $"http://{Url}/api/verify/Email" +
                    $"/{HttpUtility.UrlEncode(userIdentity.Id)}/{HttpUtility.UrlEncode(emailVerificationCode)}";

               
                return new Result
                {
                    ErrorMsg = null,
                    Response = true,
                    Body = JsonConvert.SerializeObject(new
                    {
                        UID = userIdentity.Id,
                        EmailCode = emailVerificationCode,
                        Name = newUser.UserName,
                        Email = newUser.Email
                    })

                };

            }
            else
            {
                return new Result
                {
                    ErrorMsg = "",
                    Response = false,
                    Body = null
                };
            }

          

        }

        #endregion

        #region ModificarUsuario
        /// <summary>
        /// De la misma forma que el <see cref="Crear Usuario"/> Update modifica los datos pasados por parámetro.
        /// </summary>
        /// <param name="dto">Se pide como parámetro <see cref="UserDto"/> y luego se mapea  a la entidad <see cref="Users"/></param>
        /// <returns></returns>

        public async Task<Result> UpdateUserAsync(UserDto dto, string userLogged)
        { 
            //Buscamos el usuario por el nombre del usuario que se encuentra Loggeado
            // NOTA: Se pasa el nombre del usuario que se encuentra loggeado porque si el usuario realizó algún
            // cambio en su nombre de usuario la búsqueda siempre devolvera un NULL.

            var foundUser = await _userManager.FindByNameAsync(userLogged);

            //booleano para indicar si se cambió el email, en caso de cambiarse se mandará nuevamente 
            //el mail de verificación

            bool emailChanged = false;


            ///<summary>
            ///Si no encuentra al usuario se devolverá un <see cref="Result"/> 
            ///con el mensaje de error correspondiente.
            /// </summary>

            if (foundUser == null)
                return new Result
                {
                    ErrorMsg = "No se encontró el usuario",
                    Body = null,
                    Response = false
                };

            //Si el nombre del dto no es nulo...
            if (dto.FirstName != null)
                foundUser.FirstName = dto.FirstName;
            //Si el apellido del dto no es nulo..
            if (dto.LastName != null)
               foundUser.LastName = dto.LastName;
            //Si el email no es nulo y es distinto del email registrado...
            if (dto.Email != null && !string.Equals(dto.Email.Replace(" ", ""), foundUser.NormalizedEmail))
                foundUser.Email = dto.Email;
                //Cambiamos la confirmación del usuario  
               foundUser.EmailConfirmed = false;
               //E indicamos al booleano que se cambió el email...
                emailChanged = true;
            //Si el nombre de usuario no es nulo...
            if (dto.UserName != null)
                foundUser.UserName = dto.UserName;

            //Si el mail cambió, se mandará nuevamente el mail de verificación

            if(emailChanged)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(foundUser);

                await VerifyEmailAsync(foundUser.Id, token);
            }

            //Se actualizará al usuario correspondiente

            var result = await _userManager.UpdateAsync(foundUser);

            ///<summary>
            ///Si el resultado es exitóso se mandará el <see cref="Result"/> con los datos del usuario nuevos.
            /// </summary>

            if (result.Succeeded)
                return new Result
                {
                    Response = true,
                    ErrorMsg = null,
                    Body = JsonConvert.SerializeObject(foundUser)

                };

            //Si no, se mandará un mensaje de error

            return new Result
            {
                Response = false,
                Body = null,
                ErrorMsg = "Ocurrió un error al actualizar al usuario"

            };
            



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
        public async Task<Result> LoginAsync(LoginDto credenciales)
        {
            var error = new Result
            {
                Body = null,
                Response = false,
                ErrorMsg = "Usuario o Password Inválidos"
            };

            if (credenciales?.UsuarioOEmail == null || string.IsNullOrWhiteSpace(credenciales.UsuarioOEmail))
                return error;

            var isEmail = credenciales.UsuarioOEmail.Contains("@");

            var user = isEmail ? await _userManager.FindByEmailAsync(credenciales.UsuarioOEmail) :
                await _userManager.FindByNameAsync(credenciales.UsuarioOEmail);

            if (user == null)
                return error;


            var isValidPassword = await _userManager.CheckPasswordAsync(user, credenciales.Password);

            if (!isValidPassword)
                    return error;

            var userName = user.UserName;

            return new Result
            {
                Response = true,
                ErrorMsg = null,
                Body = JsonConvert.SerializeObject(new
                {
                    Email = user.Email,
                    Username = user.UserName,
                    Token = user.GenerateJwtTokens()
                })


            };
            

        }
        #endregion

        #region Cerrar Sesión
        //Método para cerrar la sesión.
        //public async Task SignOutAsync()
        //{
        //   await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        //}

        #endregion

        #region Método para Verificar el Email
        public async Task<Result> VerifyEmailAsync(string userId, string emailToken)
        {
            var user = await _userManager.FindByIdAsync(userId);


            emailToken = emailToken.Replace("%2F", "/").Replace("%2F", "/");
           
            emailToken = emailToken.Replace("%2B", "+").Replace("%2B", "+");

            emailToken = emailToken.Replace("%3D", "=").Replace("%3D", "=");

            var resultError = new Result
            {
                ErrorMsg = "Error al encontrar el usuario",
                Body = null,
                Response = false 
            };


            //Si el usuario no existe

            if (user == null)
                return resultError;

            if (user.EmailConfirmed)
            {
                resultError.ErrorMsg = "Su Mail ya se encuentra verificado";
                return resultError;
            }
                

            //Si el usuario existe:
            //Verificamos el token del email

            var result = await _userManager.ConfirmEmailAsync(user, emailToken);

            //Si fue exitosa la operacion
            if (result.Succeeded)
                return new Result
                {
                   ErrorMsg = null,
                   Body = "Mail Verficado",
                   Response = true
                };

            resultError.ErrorMsg = "Token Inválido";

            return resultError;

        }
        #endregion


        public async Task<Result> GetUserProfile(ClaimsPrincipal userName)
        {
            var user = await _userManager.GetUserAsync(userName);


            if (user != null)
                return new Result
                {
                    Response = true,
                    ErrorMsg = null,
                    Body = JsonConvert.SerializeObject(user)
                 };


            return new Result
            {
                Response = false,
                ErrorMsg = "No se encontró el usuario",
                Body = null
            };
        }

    }
}
