using Infrastructure.Application.Templates.DataTransferObjects;
using Utils.IO.Templates;
using ApplicationLib.DataTransferObjects;
using ApplicationLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Application.Templates
{

    public class TemplatesApplication : ITemplatesApplication
    {


        #region Propiedades

        ///// <summary>
        ///// Manejador de plantillas
        ///// </summary>
        private ITemplatesManager templatesManager;


        #endregion



        #region Funciones

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="templatesManager"></param>
        public TemplatesApplication(ITemplatesManager templatesManager)
        {

            this.templatesManager = templatesManager;

        }

        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="twrs"></param>
        /// <returns></returns>
        public ApiResultBase AddSections(TemplateWebRequests twrs)
        {
            //Validacion a nivel aplicacion web (web application) 1
            if (twrs == null)
            {
                throw new BadRequestException("Bad request");
            }
            else
            {
                //Validacion a nivel aplicacion web (web application) 2
                if (twrs.TemplateOperationCommands == null || twrs.TemplateOperationCommands.Count == 0)
                {
                    throw new BadRequestException("No operations commands to make");
                }
                else
                {
                    ApiResultBase apiResult = new ApiResultBase();

                    try
                    {
                        string[] templatesIds = (from asr in twrs.TemplateOperationCommands select asr.TemplateId).ToArray();
                        int[] templatesTypes = (from asr in twrs.TemplateOperationCommands select asr.TemplateType).ToArray();
                        int[] atTheEndsTypes = (from asr in twrs.TemplateOperationCommands select asr.AtTheEndType).ToArray();
                        string key = twrs.Key;


                        //Se agregan las secciones de acuerdo a los solicitudes en addSectionApplicationRequests
                        //Aqui se puede disparar una excepcion a nivel web application
                        Template template = this.templatesManager.AddSections(twrs.TemplateId, twrs.TemplateType, key, templatesIds, templatesTypes, atTheEndsTypes);


                        apiResult.IsSuccess = template != null ? true : false;
                        apiResult.UserMessage = template != null ? "Secciones agregadas coorectamente" : "Las secciones no se agregaron correctamente";
                        apiResult.ApplicationMessage = template != null ? ApiResultBase.SUCCESS : ApiResultBase.ERROR;
                        apiResult.ResultCode = template != null ? (int)ApiResultCodes.SUCESS : (int)ApiResultCodes.ERROR;
                        apiResult.Data = template != null ? new ReplaceContentResultDTO() { TemplateId = template.Id.ToString(), Token = twrs.Token } : new ReplaceContentResultDTO() { TemplateId = "-1", Token = twrs.Token };
                        
                    }
                    catch (Exception ex)
                    {
                        //Log Ex

                        //Solo en caso de que el ambiente sea de desarrollo o calidad se envia el error
                        //if(environment == "DEV" || environment == "QA")
                        //apiResult.Error = ex;

                        apiResult.IsSuccess = false;
                        apiResult.UserMessage = ApiResultBase.ERROR;
                        apiResult.ApplicationMessage = ApiResultBase.ERROR;
                        apiResult.ResultCode = 1;
                    }

                    return apiResult;

                }
            }
        }





        ///// <summary>
        ///// Manejador de plantillas exclusivo para el reporte de consumos
        ///// </summary>
        //private IConsumptionsTemplateManager consumptionsTemplateManager;




        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="consumptionsTemplateManager"></param>
        //public TemplatesApplication(IConsumptionsTemplateManager consumptionsTemplateManager)
        //{
        //    this.consumptionsTemplateManager = consumptionsTemplateManager;
        //    this.templateManager             = new TemplateManager();
            
        //}



     
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="templateId"></param>
        ///// <param name="templateType"></param>
        ///// <param name="keys"></param>
        ///// <param name="values"></param>
        ///// <param name="keepKeyInContent"></param>
        ///// <returns></returns>
        //public Template CloneTemplate(long templateId, int templateType,  string[] keys, string[] values, bool[] keepContentAtTheEnd)
        //{
        //    try
        //    {
        //        return templateManager.CloneTemplate(templateId, GetTemplateType(templateType), keys, values, keepContentAtTheEnd);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="templateId"></param>
        ///// <param name="templateType"></param>
        ///// <param name="keysValues"></param>
        ///// <param name="keepKeyInContent"></param>
        ///// <returns></returns>
        //public Template CloneTemplate(long templateId, TemplateType templateType, List<TemplateKeyValue> keysValues)
        //{
        //    try
        //    {
        //        return templateManager.CloneTemplate(templateId, templateType, keysValues);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="templateId"></param>
        ///// <param name="templateType"></param>
        ///// <param name="keysValue"></param>
        ///// <returns></returns>
        //public Template CloneTemplate(long templateId, TemplateType templateType, TemplateKeyValue keysValue)
        //{
        //    try
        //    {
        //        return templateManager.CloneTemplate(templateId, templateType, keysValue);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="templateType"></param>
        ///// <param name="key"></param>
        ///// <param name="Content"></param>
        ///// <returns></returns>
        //private Template AddSection(long id, TemplateType templateType, string key, string Content)
        //{
        //    Template template = templateManager.GetTemplateByTemplateIdAndType(id, templateType);
        //    return templateManager.AddSection(template, new TemplateKeyValue() { Key = key, ValueString = Content });
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="template"></param>
        ///// <param name="key"></param>
        ///// <param name="content"></param>
        ///// <returns></returns>
        //public Template AddSection(Template template, string key, Template sourceTemplate, bool keepKeyInContent)
        //{
        //    return templateManager.AddSection(template, new TemplateKeyValue() { Key = key, ValueString = sourceTemplate.Content, KeepKeyContent = keepKeyInContent });
        //}



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="template"></param>
        ///// <param name="templateKeyValues"></param>
        ///// <returns></returns>
        //public Template AddSections(Template template, string key, List<Template> templates)
        //{
        //    List<TemplateKeyValue> templateKeyValues = new List<TemplateKeyValue>();
        //    for (int i = 0; i < templates.Count; i++)
        //    {
        //        templateKeyValues.Add(new TemplateKeyValue() { ValueString = templates[i].Content, Key = key, KeepKeyContent = true });
        //    }
            
            
        //    return templateManager.AddSections(template, templateKeyValues);
        //}



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="templateId"></param>
        ///// <param name="templateType"></param>
        ///// <param name="addSectionApplicationRequests"></param>
        ///// <returns></returns>
        //public Template AddSections(long templateId, int templateType, List<AddSectionApplicationRequest> addSectionApplicationRequests)
        //{

        //    TemplateType localTemplateType = GetTemplateType(templateType);

        //    Template templateWhichWillAddTheSections = templateManager.GetTemplateByTemplateIdAndType(templateId, localTemplateType);

        //    for (int i = 0; i < addSectionApplicationRequests.Count; i++)
        //    {
        //        AddSectionApplicationRequest aarequest = addSectionApplicationRequests.ElementAt(i);
        //        Template templateWithContentToAdd = templateManager.GetTemplateByTemplateIdAndType(aarequest.TemplateId, aarequest.TemplateType);
        //        TemplateKeyValue templateKeyValue = new TemplateKeyValue() { ValueString = templateWithContentToAdd.Content, Key = aarequest.Key, KeepKeyContent = i < addSectionApplicationRequests.Count - 1? true: false };
        //        templateWhichWillAddTheSections = templateManager.AddSection(templateWhichWillAddTheSections, templateKeyValue);
        //    }

        //    return templateWhichWillAddTheSections;
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="tar"></param>
        ///// <returns></returns>
        //public Template CloneConsumptionsTemplate(List<TemplateApplicationRequest> tar)
        //{
        //    try
        //    {
        //        return null;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="keys"></param>
        ///// <param name="values"></param>
        ///// <returns></returns>
        //public List<TemplateKeyValue> GetTemplateKeyValues(string[] keys, string[] values, bool[] keepContent, bool[] keepKeyInContent)
        //{

        //    if ( (keys != null && values != null) && (keys.Length == values.Length))
        //    {
        //        List<TemplateKeyValue> templateKeyValues = new List<TemplateKeyValue>();

        //        if ((keepContent != null && keepKeyInContent != null) && (keepContent.Length == keys.Length) && (keepContent.Length == keepKeyInContent.Length))
        //        {

        //            for (int i = 0; i < keys.Length; i++)
        //            {
        //                templateKeyValues.Add(new TemplateKeyValue() { Key = keys[i], ValueString = values[i], KeepContent = keepContent[i], KeepKeyContent = keepKeyInContent[i] });
        //            }
        //        }
        //        else if (keepContent != null && keepContent.Length == keys.Length)
        //        {
        //            for (int i = 0; i < keys.Length; i++)
        //            {
        //                templateKeyValues.Add(new TemplateKeyValue() { Key = keys[i], ValueString = values[i], KeepContent = keepContent[i] });
        //            }
        //        }
        //        else if (keepKeyInContent != null && keepKeyInContent.Length == keys.Length)
        //        {
        //            for (int i = 0; i < keys.Length; i++)
        //            {
        //                templateKeyValues.Add(new TemplateKeyValue() { Key = keys[i], ValueString = values[i], KeepKeyContent = keepKeyInContent[i] });
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < keys.Length; i++)
        //            {
        //                templateKeyValues.Add(new TemplateKeyValue() { Key = keys[i], ValueString = values[i] });
        //            }
        //        }


        //        return templateKeyValues;
        //    }

        //    return null;
            
        //}





    }
}
