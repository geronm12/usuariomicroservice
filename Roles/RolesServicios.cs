using MicroServicioUsuarios.Data;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Roles
{
    public class RolesServicios: IRolesServicios
    {
        protected RoleManager<Roles> _roleManager;

       
        public RoleResult resultError
        {
            get
            {
                return new RoleResult { Succesfull = false, Body = null, Errores = new List<ErrorMsg>() };
            }
            set
            {
                resultError = value;
            }

        }


        /// <summary>
        /// Clase que implementa <see cref="RoleManager{TRole}"/>
        /// Y la interfaz <see cref="IRolesServicios"/>
        /// </summary>
        /// <param name="roleManager"></param>
        public RolesServicios(RoleManager<Roles> roleManager)
        {
            _roleManager = roleManager;

        }

        /// <summary>
        /// Método para crear Roles
        /// </summary>
        /// <param name="rol">Rol Dto: Data transfer object para envair un Rol desde la IU</param>
        /// <returns>Retorna <see cref="RoleResult"/>En caso de éxtio o fracaso de la operación.</returns>
        #region Crear Rol
        public async Task<RoleResult> Create(RolDto rol)
        {
            //Mappeamos el RolDto a Rol Entity

            Roles _rol = new Roles
            {
                Name = rol.Name,
            };

            //Creamos el rol y lo insertamos en la base de datos

            var result = await _roleManager.CreateAsync(_rol);

            //Si la operación es exitosa retornamos un RoleResult con un StatusCode.Ok y Un Mensaje

            if (result.Succeeded)
                return new RoleResult
                {
                    Errores = null,
                    Body = JsonConvert.SerializeObject(new {http = HttpStatusCode.OK, Mensaje = "Rol Creado Con éxito" }),
                    Succesfull = true
                };


            //De lo contrario creamos un resultado de error
            //Recorremos la lista de errores en result

            foreach (var item in result.Errors)
            {

                //Casteamos de forma implícita a ErrorMsg
                resultError.Errores.Add((ErrorMsg)item);

            }
           
             //Retornamos el ResultError
            return resultError;


        
        
        }
        #endregion

        /// <summary>
        /// Método para eliminar Roles
        /// </summary>
        /// <param name="rol"></param>
        /// <returns>Retorna <see cref="RoleResult"/>En caso de éxito o fracaso de la operación</returns>
        #region Eliminar Rol

        public async Task<RoleResult> Delete(RolDto rol)
        {
            var entity = await _roleManager.FindByNameAsync(rol.Name);



            if (entity == null)
            {
                resultError.Errores.Add(new ErrorMsg { ErrorCode = HttpStatusCode.NotFound.ToString(), ErrorMessagge = "No se encontró el rol" });
                return resultError;
            }
            else
            {
                var result = await _roleManager.DeleteAsync(entity);

                if (result.Succeeded)
                    return new RoleResult
                    {
                        Errores = null,
                        Succesfull = true,
                        Body = JsonConvert.SerializeObject(new { Http = HttpStatusCode.OK, Mensaje = "Se eliminó con éxito el Rol" })

                    };

                foreach (var item in result.Errors)
                {
                    resultError.Errores.Add((ErrorMsg)item);
                }

                return resultError;
            }
                

           

        }
        #endregion

        #region Buscar por Id
        // Método que busca al Rol por Id
        public async Task<RoleResult> FindByIdAsync(string roleId)
        {
            var rol = await _roleManager.FindByIdAsync(roleId);

            if (rol == null)
            {
                resultError.Errores.Add(new ErrorMsg { ErrorCode = HttpStatusCode.NotFound.ToString(), ErrorMessagge = "No se encontró el rol" });
                return resultError;
            }
            else
            {
                return new RoleResult
                {
                    Errores = null,
                    Body = JsonConvert.SerializeObject(rol),
                    Succesfull = true
                };
            }
                




        }
        #endregion

        #region Buscar por Nombre
        public async Task<RoleResult> FindByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                resultError.Errores.Add
                    (new ErrorMsg 
                    { ErrorCode = HttpStatusCode.NotFound.ToString(), ErrorMessagge = "No se encontró el Rol " });
                return resultError;
            }
            else
            {
                 return new RoleResult
                {
                     Succesfull = true,
                     Errores = null,
                     Body = JsonConvert.SerializeObject(role)
                };
            }



        }
        #endregion

        #region Modificar Rol
        public async Task<RoleResult> UpdateAsync(RolDto dto)
        {
            var rol = await _roleManager.FindByIdAsync(dto.Id);

            if (rol == null)
            {
                resultError.Errores.Add(new ErrorMsg
                {
                    ErrorCode = HttpStatusCode.NotFound.ToString(),
                    ErrorMessagge = "No se encontró el Rol"
                });
            }
            else
            {
                rol.Name = dto.Name;
                var result = await _roleManager.UpdateAsync(rol);

                if (result.Succeeded)
                {
                    return new RoleResult
                    {
                        Errores = null,
                        Succesfull = true,
                        Body = JsonConvert.SerializeObject(
                       new { Http = HttpStatusCode.OK, Mensaje = "Rol Modificado con éxito" })
                    };
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        resultError.Errores.Add((ErrorMsg)item);
                    }

                    return resultError;
                }

                
            }

            resultError.Errores.Add(new ErrorMsg
            {
                ErrorCode = HttpStatusCode.BadRequest.ToString(),
                ErrorMessagge = "Ocurrió un error" +
                "al intentar modificar el  Rol"
            });
            return resultError;

        }
        #endregion
    }
}
