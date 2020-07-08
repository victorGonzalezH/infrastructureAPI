using System;
using System.Collections.Generic;
using System.Text;


namespace Infrastructure.Application.Templates.DataTransferObjects
{

    /// <summary>
    /// Clase para una operacion basica en un template
    /// </summary>
    public class TemplateOperationCommand
    {

        /// <summary>
        /// Identificador de la plantilla en la que se desea reemplazar los valores
        /// </summary>
        public string TemplateId { get; set; }
        //public long TemplateId { get; set; }

        /// <summary>
        /// Tipo de la plantilla
        /// </summary>
        public int TemplateType { get; set; }


        /// <summary>
        /// Tipo de contenido que se dejara a lo ultimo del reemplazo de cada valor. Cuando se reemplaza una palabra
        /// clave por un valores, a veces es necesario mantener la misma llave despues de la sustitucion, o el mismo
        /// contenido de la plantilla. Con esta propiedad se indica que tipo de llave se deja
        /// </summary>
        public int AtTheEndType { get; set; }



        /// <summary>
        /// Identificador de la operacion
        /// </summary>
        public string Token { get; set; }
        //public long Token { get; set; }

    }
}
