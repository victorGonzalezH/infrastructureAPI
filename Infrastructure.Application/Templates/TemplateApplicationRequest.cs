using Utils.IO.Templates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Application.Templates
{

    public class TemplateApplicationRequest
    {


       /// <summary>
       /// 
       /// </summary>
        public long TemplateId { get; set; }


        /// <summary>
        /// Tipo de la plantilla
        /// </summary>
        public TemplateType TemplateType { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public List<TemplateKeyValue> TemplateKeyValues {get; set;}


    }


    /// <summary>
    /// 
    /// </summary>
    public class AddSectionApplicationRequest
    {
        /// <summary>
        /// Identificador de la plantilla
        /// </summary>
        public long TemplateId { get; set; }

        /// <summary>
        /// Tipo de la plantilla
        /// </summary>
        public TemplateType TemplateType { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

    }
}
