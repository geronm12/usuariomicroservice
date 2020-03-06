
namespace MicroServicioUsuarios.Whatsapp
{
    using MicroServicioUsuarios.Data;
    using MicroServicioUsuarios.Whatsapp.ExtensionesYHelpers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Twilio;
    using Twilio.Rest.Api.V2010.Account;
    using Twilio.Types;
    using static MicroServicioUsuarios.Servicios.Extensiones.ConfigExtension;
    
    /// <summary>
    /// WhatsappSender implementa la interfaz <see cref="IWhatsappSender"/> para poder enviar mensajes con códigos de verificacion
    /// </summary>
    
    public class WhatsappSender : IWhatsappSender
    {
        #region Protected Variables

        protected UserManager<Users> _userManager;

        protected ApplicationContext _context;

        protected WhatsappCookiesCreator _creator;

        #endregion

        #region Constructor
        public WhatsappSender(UserManager<Users> userManager, ApplicationContext context, WhatsappCookiesCreator creator)
        {
            _userManager = userManager;

            _context = context;

            _creator = creator;
         

        }
        #endregion

        /// <summary>
        /// Método que genera el código de verificación y envía el mensaje.
        /// </summary>
        /// <param name="modeloMensaje">Dto que posee los datos para poder utilizar el método</param>
        /// <param name="método">Tipo de forma de verificación que desea el usuario</param>
        /// <returns><see cref="MensajeResult"/> Con los datos de operación exitósa o error.</returns>
        
        #region Método para Enviar Mensaje
        public async Task<MensajeResult>EnviarMensaje(WhatsappSenderDto modeloMensaje, FormaVerificacion método, string email)
        {
            //Iniciamos el cliente de Twilio con nuestro User Id proporcionado por la página y la apiKey

            TwilioClient.Init(_config["Twilio:UID"], _config["Twilio:ApiKey"]);

            //Usamos la extenisón de WhastappSenderDto para llamar al método que genera el código de forma aleatoria

            var codigo = modeloMensaje.GenerarCodigoVerificacion();
            

            //Verificamos si el usuario es un email o el nombre de Usuario y lo buscamos.

            var user = email.Contains("@") ? await _userManager.FindByEmailAsync(email) : await
                _userManager.FindByNameAsync(email);

            //Si no se encontró el  usuario
            //Devolvemos un MensajeResult con los datos del error
            if (user == null)
                return new MensajeResult
                {
                    MsgError = "No se encontró el usuario al cual enviarle un sms",
                    Body = null,
                    Succesful = false
                };

            //En caso de que el usuario SI se encuentre
            //Verificamos que forma de verificación está usando el usuario y entramos al switch

            switch (método)
            {
                //Caso de que elija Whatsapp

                case FormaVerificacion.Whatsapp:
                    ///<summary>
                    //Invocamos la API y mentira MessageResource creamos al mensaje
                    ///<param>from: Mensaje desde donde saldrá el mensaje, éste debe ser nuestro n° twilio</param>
                    ///<param>body: Cuerpo del mensaje que enviaremos en este caso el mensaje más el código</param>
                    ///<param>to: teléfono de la persona a la que se le enviará el mensaje 
                    ///NOTA: El formato del n° de teléfono es E 164
                    /// <see cref="https://www.twilio.com/docs/glossary/what-e164"/>
                    /// </summary>
                    var message = await MessageResource.CreateAsync(
                     from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                     body: modeloMensaje.Mensaje + codigo,
                     to: new Twilio.Types.PhoneNumber($"whatsapp:{modeloMensaje.Telefono}"));

                    //Si el Status del mensaje es Queued(solicitado) 
                    //NOTA: Habría que crear un método capaz de ir al servidor y verificar si el status ya es entregado
                    //o si ocurrió algún error
                    if (message.Status == MessageResource.StatusEnum.Queued)
                    {

                        //Invocamos al método para colocar el código den los cookies
                         _creator.Set(codigo);

                         //Guardamos el teléfono en el usuario
                         user.PhoneNumber = modeloMensaje.Telefono.ToString();
                         //Modificamos el usuario en la DB
                         await _userManager.UpdateAsync(user);

                        //Enviamos un Result con el objeto serializado a Json
                        return new MensajeResult
                        {
                            Succesful = true,
                            MsgError = null,
                            Body = JsonConvert.SerializeObject(new TwilioResponse
                            {
                                Body = message.Body,
                                From = message.From.ToString(),
                                To = message.To.ToString(),
                                Codigo = codigo

                            })
                        };
                    }

                        

                    return new MensajeResult
                    {
                        Succesful = false,
                        MsgError = "Hubo un error al enviar su mensaje",
                        Body = JsonConvert.SerializeObject(message.ErrorMessage + message.ErrorCode.ToString())
                    };

                    case FormaVerificacion.SMS:
                    var verificationSms = await MessageResource.CreateAsync(
                    from: new PhoneNumber("+14155992671"),
                    to: new PhoneNumber("+12069419717"),
                    messagingServiceSid: _config["Twilio:MSID"],
                    body: modeloMensaje.Mensaje + modeloMensaje.GenerarCodigoVerificacion());

                    if(verificationSms.Status == MessageResource.StatusEnum.Queued)
                    return new MensajeResult
                    {
                        Succesful = true,
                        MsgError = null,
                        Body = JsonConvert.SerializeObject(new TwilioResponse
                        {
                            Body = verificationSms.Body,
                            From = verificationSms.From.ToString(),
                            To = verificationSms.To.ToString(),
                            Codigo = codigo

                        })

                    };

                    return new MensajeResult
                    {
                        MsgError = "Hubo un error al enviar su mensaje",
                        Body = JsonConvert.SerializeObject(verificationSms.ErrorMessage + verificationSms.ErrorCode.ToString()),
                        Succesful = false
                    };

                 
                
                    default:
                    return new MensajeResult
                    {
                        MsgError = "El método al que intenta acceder no existe",
                        Body = null,
                        Succesful = false
                    };
            }


            

            

        }

        #endregion

        #region Método para verificar el código
        /// <summary>
        /// Método con el cual verificaremos si el código utilizado es coincidente con el de la base de datos y cambiaremos
        /// el booleano que indica que el teléfono ya fue verificado a true.
        /// </summary>
        /// <param name="modeloMensaje">Dto que pasa los datos del usuario</param>
        /// <returns><see cref="MensajeResult"/> que indica si fue verificado correctamente o no el número.</returns>
        public async Task<MensajeResult> VerificarCódigo(WhastappUserVerificationDto modeloMensaje)
        {
            var user = modeloMensaje.NombreUsuarioOEmail.Contains("@")? 
                await _userManager.FindByEmailAsync(modeloMensaje.NombreUsuarioOEmail) : 
             await _userManager.FindByNameAsync(modeloMensaje.NombreUsuarioOEmail);

            var resultError = new MensajeResult
            {
                Body = null,
                MsgError = "Ocurrió un error al verificar el código",
                Succesful = false
            };



            if(user != null)
            {

                string codigo = _creator.GetKey();

                if(codigo== modeloMensaje.Codigo)
                {
                    user.PhoneNumberConfirmed = true;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                        return new MensajeResult
                        {
                            MsgError = null,
                            Body = "Su código fue verificado con éxito",
                            Succesful = true

                        };

                    return resultError;
                }
            }

            resultError.MsgError = "No se encontró al usuario";
            return resultError;
        }

        #endregion
    }
}
