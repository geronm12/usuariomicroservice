using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Roles
{
    public class RoleResult
    {
        public RoleResult()
        {
            if(Errores.Count <= 0)
            {
                Errores = new List<ErrorMsg>();
            }

        }

        public bool Succesfull { get; set; }


        public string Body { get; set; }


        public List<ErrorMsg> Errores { get; set; }


        

    }
}
