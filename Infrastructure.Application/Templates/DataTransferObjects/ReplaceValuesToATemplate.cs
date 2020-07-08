using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Application.Templates.DataTransferObjects
{
    /// <summary>
    /// Comando para reemplazar valores en una plantilla.
    /// </summary>
    public class ReplaceValuesToATemplate : TemplateOperationCommand
    {


        /// <summary>
        /// Palabras clave (llaves) que se encuentran dentro del contenido de la plantilla en donde se sustituiran
        /// los valores
        /// </summary>
        public string[] TemplateKeys { get; set; }


        /// <summary>
        /// Valores a reemplazar
        /// </summary>
        public string[] TemplateValues { get; set; }



        /// <summary>
        /// Bandera que indica si el tipo de contenido se deja o no
        /// </summary>
        public bool[] KeepAtTheEndType { get; set; }


    }
}
