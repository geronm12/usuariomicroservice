using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.ViewModels
{
    public class TResult<T>
    {

        public bool Succesful => ErrorMsg == null;

        public T Response { get; set; }

        public string ErrorMsg { get; set; }
    }
}
