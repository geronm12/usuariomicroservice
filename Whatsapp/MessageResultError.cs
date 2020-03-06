using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.ViewModels
{
    public class MessageResultError
    {
        public string ErrorMsg { get; set; }

        public bool Succesful => ErrorMsg == null;

        public string ErrorBody { get; set; }

    }
}
