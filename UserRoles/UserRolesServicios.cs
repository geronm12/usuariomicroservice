 namespace MicroServicioUsuarios.UserRoles
{
    using global::AutoMapper;
    using MicroServicioUsuarios.Data;
    using MicroServicioUsuarios.Roles;
    using MicroServicioUsuarios.Servicios.UserService;
    using Microsoft.AspNetCore.Identity;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using static AutoMapper.AutoMapperService;
    using static RolDtoExtensionMethod;
    public class UserRolesServicios: IUserRolesServicios
    {
        protected UserManager<Users> _userManager;

        protected RoleManager<Roles> _roleManager;

        private bool IsInRole;

        private UserRolResult errorResult {
            get
            {
                return new UserRolResult
                {
                    Body = null,
                    Errores = new List<ErrorMsg>(),
                    Roles = null,
                    Succesfull = false,
                    Usuario = null
                };
            }

            set
            {
                errorResult = value;
            }
        }

        private IMapper _mapper;

        private IRolesServicios _service;
        public UserRolesServicios(UserManager<Users> userManager, RoleManager<Roles> roleManager, IRolesServicios service )
        {
            _userManager = userManager;

            _roleManager = roleManager;

            _mapper = CrearMapa();

            _service = service;
 
        }

        #region Asignar Rol
        public async Task<UserRolResult> AddToRolAsync(string userNameOrEmail, string roleName)
        {
            IsInRole = await _roleManager.RoleExistsAsync(roleName);

            if (!IsInRole)
            {
                errorResult.Errores.Add(new ErrorMsg
                {
                    ErrorCode = HttpStatusCode.NotFound.ToString()
                    ,
                    ErrorMessagge = "No se encontró el rol al que desea asignar el usuario"
                });

                return errorResult;
            }

            Users usuario = userNameOrEmail.Contains("@") ? await _userManager.FindByEmailAsync(userNameOrEmail)
                : await _userManager.FindByNameAsync(userNameOrEmail);

            if (usuario == null)
            {
                errorResult.Errores.Add(new ErrorMsg { ErrorCode = HttpStatusCode.OK.ToString(), ErrorMessagge = "No se encontró al Usuario" });
                return errorResult;
            }



            var result = await _userManager.AddToRoleAsync(usuario, roleName);

            if (result.Succeeded)
            {
                return new UserRolResult
                {
                    Errores = null,
                    Roles = new List<RolDto> { new RolDto { Name = roleName } },
                    Succesfull = true,
                    Usuario = _mapper.Map<UserDto>(usuario),
                    Body = JsonConvert.SerializeObject(new
                    {
                        Http = HttpStatusCode.OK,
                        Mensaje = "Se Asignó el Rol con éxito."
                    })

                };
            }

            errorResult.Errores.Add(new ErrorMsg
            {
                ErrorCode = HttpStatusCode.BadRequest.ToString()
                ,
                ErrorMessagge = "Hubo un error al asignar el Rol"
            });
            return errorResult;
               

             

        }
        #endregion

        #region Asignar Roles
        public async Task<UserRolResult> AddToRolesAsync(string userNameOrEmail, List<string> roles)
        {
            Dictionary<string,bool> verificar = new Dictionary<string,bool>();
             
            foreach (var item in roles)
            {
                IsInRole = await _roleManager.RoleExistsAsync(item);

                 verificar.Add(item, IsInRole);
            }

            var hayFalse = verificar.Where(x => x.Value == false);

            if (hayFalse.Count() > 0)
            {
                foreach (var item in hayFalse)
                {
                    errorResult.Errores.Add(new ErrorMsg
                    {
                        ErrorCode = HttpStatusCode.NotFound.ToString(),
                        ErrorMessagge = $"No se encontró el rol {item.Key}"
                    });

                }
                

                return errorResult;
            }

            Users usuario = userNameOrEmail.Contains("@") ? await _userManager.FindByEmailAsync(userNameOrEmail)
                : await _userManager.FindByNameAsync(userNameOrEmail);

            if (usuario == null)
            {
                errorResult.Errores.Add(new ErrorMsg { ErrorCode = HttpStatusCode.OK.ToString(), ErrorMessagge = "No se encontró al Usuario" });
                return errorResult;
            }
               



            var result = await _userManager.AddToRolesAsync(usuario, roles);

            if (result.Succeeded)
            {
                var listaRoles = new List<RolDto>();

                foreach (var item in roles)
                {
                    listaRoles.Add(new RolDto { Name = item });

                }

                return new UserRolResult
                {
                    Errores = null,
                    Roles = listaRoles,
                    Succesfull = true,
                    Usuario = _mapper.Map<UserDto>(usuario),
                    Body = JsonConvert.SerializeObject(new
                    {
                        Http = HttpStatusCode.OK,
                        Mensaje = "Se Asignaron los Roles con éxito."
                    })

                };
            }

            errorResult.Errores.Add(new ErrorMsg
            {
                ErrorCode = HttpStatusCode.BadRequest.ToString(),
                ErrorMessagge = "Hubo un error al asignar el Rol"
            });
            return errorResult;

        }
        #endregion

        #region Obtener Roles de un Usuario
        public async Task<UserRolResult> GetRolesAsync(string userNameOrEmail)
        {
            var usuario = userNameOrEmail.Contains("@") ?
                await _userManager.FindByEmailAsync(userNameOrEmail)
                : await _userManager.FindByNameAsync(userNameOrEmail);

            if (usuario == null)
            {
                errorResult.Errores.Add(new ErrorMsg
                {
                    ErrorCode = HttpStatusCode.NotFound.ToString(),
                    ErrorMessagge = "No se encontró el usuario"

                });
                return errorResult;
            }
                
            var roles = await _userManager.GetRolesAsync(usuario);

            if(roles.Count() > 0)
            {
                return new UserRolResult
                {
                    Errores = null,
                    Body = JsonConvert.SerializeObject(roles),
                    Roles = _service.ConvertToRolDto(roles),
                    Succesfull = true,
                    Usuario = _mapper.Map<UserDto>(usuario)
                };
            }
           
            errorResult.Errores.Add(new ErrorMsg
            {
                ErrorCode = HttpStatusCode.NotFound.ToString(),
                ErrorMessagge = "El Usuario no posee roles asignados"
            
            });

            return errorResult;

        }

        #endregion

        #region Obtener Usuarios en un Rol
        public Task<UserRolResult> GetUsersInRolAsync(string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Remover de un Rol específico

        public Task<UserRolResult> RemoveFromRolAsync(string userNameOrEmail, string roleName)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Remover de Varios Roles
        public Task<UserRolResult> RemoveFromRolesAsync(string userNameOrEmail, List<string> roles)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
