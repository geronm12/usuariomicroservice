namespace Mailer
{
    /// <summary>
    /// Error de respuesta de la clase SendGrid
    /// </summary>
    public class SendGridErrorResponse
    {
        /// <summary>
        /// Mensaje de error 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// El campo que posee el error (campo a corregir)
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Ayuda para solucionar el error
        /// </summary>
        public string Help { get; set; }

    }
}
