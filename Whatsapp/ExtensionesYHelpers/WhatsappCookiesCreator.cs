using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Whatsapp.ExtensionesYHelpers
{
    public class WhatsappCookiesCreator: Cookies.HttpCookiesCreator
    {
        private readonly IHttpContextAccessor _accesor;

        protected string llave => "CookieWhatsapp";

        protected int expireTime => 30;

        public WhatsappCookiesCreator(IHttpContextAccessor accesor): base(accesor)
        {
            _accesor = accesor;
        }

        public string GetKey()
        {
            return Get(llave);
        }


        public override string Get(string key)
        {
            return base.Get(llave);
        }

        public override void Set(string value)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(expireTime)
            };

            _accesor.HttpContext.Response.Cookies.Append(llave, value, options);

        }

        public override void Remove(string key)
        {
            base.Remove(key);
        }

    }
}
