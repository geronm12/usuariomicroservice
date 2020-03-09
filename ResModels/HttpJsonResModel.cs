using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.ResModels
{
    public class HttpJsonResModel<T>
    {
        public HttpStatusCode Http { get; set; }

        public string Mensaje { get; set; }

        public T Objeto { get; set; }

        public bool Succesfull { get; set; }
         


    }
}
