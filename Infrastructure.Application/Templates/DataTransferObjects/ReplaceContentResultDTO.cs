using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Application.Templates.DataTransferObjects
{
    public class ReplaceContentResultDTO
    {

        /// <summary>
        /// 
        /// </summary>
        public string TemplateId { get; set; }
        

        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }
        


        /// <summary>
        /// Tipo de archivo de la plantilla
        /// </summary>
        public int Type { get; set; }


    }
}
