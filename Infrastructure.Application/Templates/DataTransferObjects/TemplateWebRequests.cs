using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Application.Templates.DataTransferObjects
{


    public class TemplateWebRequests
    {
        /// <summary>
        /// 
        /// </summary>
        //public long TemplateId { get; set; }
        public string TemplateId { get; set; }

        /// <summary>
        /// Tipo de la plantilla
        /// </summary>
        public int TemplateType { get; set; }


        /// <summary>
        /// Identificador de la operacion
        /// </summary>
        public string Token { get; set; }
        //public long Token { get; set; }


        /// <summary>
        /// Clave de la plantilla donde se sustituiran los valores
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ReplaceValuesToATemplate> ReplaceValuesRequests { get; private set; }


        /// <summary>
        ///Listado de operaciones basicas para las plantillas
        /// </summary>
        public List<TemplateOperationCommand> TemplateOperationCommands { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        public TemplateWebRequests()
        {
            ReplaceValuesRequests = new List<ReplaceValuesToATemplate>();
            TemplateOperationCommands = new List<TemplateOperationCommand>();
        }
    }
}
