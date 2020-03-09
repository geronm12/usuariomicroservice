using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Roles
{
    public class ErrorMsg
    {

        public string ErrorMessagge { get; set; }


        public string ErrorCode { get; set; }


        public static implicit operator ErrorMsg(IdentityError error)
        {
            ErrorMsg item = new ErrorMsg();

            item.ErrorCode = error.Code;
            item.ErrorMessagge = error.Description;

            return item;

        }
    }
}
