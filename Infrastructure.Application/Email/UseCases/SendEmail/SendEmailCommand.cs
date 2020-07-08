
namespace Infrastructure.API.Application.Email.UseCases.SendEmail
{


    /// <summary>
    /// 
    /// </summary>
    public class SendEmailCommand
    {

        /// <summary>
        /// Tema del correo
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Cuerpo del correo
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Correos a los que va dirigido
        /// </summary>
        public string[] Tos { get; set; }

        /// <summary>
        /// Correos en copia
        /// </summary>
        public string[] Ccs { get; set; }

        /// <summary>
        /// Correos en copia oculta
        /// </summary>
        public string[] Bccs { get; set; }


        /// <summary>
        /// Identificador de la plantilla 
        /// </summary>
        public string TemplateId { get; set; }


        /// <summary>
        /// Indica el tipo de plantilla, si es html, txt etc
        /// </summary>
        public int TemplateTypeId { get; set; }


        /// <summary>
        /// Indica si el cuerpo del correo es HTML
        /// </summary>
        public bool IsBodyHtml { get; set; }


        /// <summary>
        /// Identificador de la operacion de envio de correo
        /// </summary>
        public string Id { get; set; }

    }
}