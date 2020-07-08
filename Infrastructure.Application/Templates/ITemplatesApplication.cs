using Infrastructure.Application.Templates.DataTransferObjects;
using Utils.IO.Templates;
using ApplicationLib.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Application.Templates
{

    public interface ITemplatesApplication
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twrs"></param>
        /// <returns></returns>
        ApiResultBase AddSections(TemplateWebRequests twrs);


    }
}
