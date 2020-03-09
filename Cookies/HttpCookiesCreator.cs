using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioUsuarios.Cookies
{
    public abstract class HttpCookiesCreator
    {

        private readonly IHttpContextAccessor _accesor;

        public HttpCookiesCreator(IHttpContextAccessor accesor)
        {
            _accesor = accesor;

        }


        public virtual void Set(string Key, string value, int? expireTime)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = expireTime.HasValue ? DateTime.Now.AddMinutes(expireTime.Value) :
                DateTime.Now.AddMinutes(30)
            };

            _accesor.HttpContext.Response.Cookies.Append(Key, value, options);

        }

        
         public virtual void Set(string Key)
         {

         }


        public virtual string Get(string key)
        {
            return _accesor.HttpContext.Request.Cookies[key];
        }

        public virtual void Remove(string key)
        {
            _accesor.HttpContext.Response.Cookies.Delete(key);
        }

         



    }
}
