using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Data
{
    public class CodigosDeVerificacion
    {  
        [Key]
        public Guid Id { get; set; }

        public string Código { get; set; }
 
        public string UserId { get; set; }
        
        //path
        public virtual Users Users { get; set; }

    }
}
