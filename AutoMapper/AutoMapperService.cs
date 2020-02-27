using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.AutoMapper
{
    public static class AutoMapperService
    {

        public static IMapper CrearMapa()
        {

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfiles>());

            var mapper = config.CreateMapper();

            return mapper;

        }



    }
}
