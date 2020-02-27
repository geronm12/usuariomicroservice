using AutoMapper;
using MicroServicioUsuarios.Data;
using MicroServicioUsuarios.Servicios.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.AutoMapper
{
    /// <summary>
    /// Clase que funciona como perfil para cargar todos los mapeos de entidad-dto-vm, etc es utilizada por <see cref="IMapper"/>
    /// y por <see cref="AutoMapper"/>
    /// </summary>


    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            //Agregamos los mapeos deseados

            CreateMap<Users, UserDto>().ReverseMap();


        }


    }
}
