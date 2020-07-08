
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
﻿using Infrastructure.Application.Email;
using Utils.Email;
using Utils.IO.Templates;
using Utils.IO.Templates.Html;
using static Utils.Email.EmailClient;

namespace Infrastructure.Application.Email
{


    /// <summary>
    /// 
    /// </summary>
    public class EmailApplication: IEmailSendCompleted, IEmailApplication
    {

        /// <summary>
        /// Cliente de correo
        /// </summary>
        private IEmailClient emailClient;

        /// <summary>
        /// Analizador de HTML
        /// </summary>
        private IHtmlParser htmlParser;


        /// <summary>
        /// 
        /// </summary>
        private ITemplatesManager templatesManager;


        /// <summary>
        /// 
        /// </summary>
        private TaskCompletionSource<bool> tcs;

        /// <summary>
        /// Indica si la ultima operacion de envio resulto exitosa o no
        /// </summary>
        private bool sendSucess;

        
        /// <summary>
        /// Indica si la configuracion de la aplicacion de correo es correcta
        /// </summary>
        private bool configOk;


        /// <summary>
        /// 
        /// </summary>
        private string hostName;


        /// <summary>
        /// 
        /// </summary>
        private int port;

        /// <summary>
        /// 
        /// </summary>
        private string userName;

        /// <summary>
        /// 
        /// </summary>
        private string password;

        /// <summary>
        /// 
        /// </summary>
        private string displayName;


        /// <summary>
        /// 
        /// </summary>
        private string webDomain;

        private bool enableSsl;

        /// <summary>
        /// 
        /// </summary>
        private string currentWorkingDirectory;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailClient"></param>
        public EmailApplication(IEmailClient emailClient, IHtmlParser htmlParser, ITemplatesManager templatesManager)
        {

            this.emailClient        = emailClient;

            this.htmlParser         = htmlParser;

            this.templatesManager = templatesManager;


            sendSucess = false;

                       
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        public void Config(string host, int port, string userName, string password, string domain, bool enableSsl)
        {
            try
            {

                emailClient.SetSmtpClientParameters(host, port);
                emailClient.SetUserName(userName);
                emailClient.SetPassword(password);
                emailClient.SetDomain(domain);
                emailClient.EnableSsl(enableSsl);
                configOk = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="displayName"></param>
        /// <param name="domain"></param>
        /// <param name="webDomain"></param>
        public EmailApplication(string hostName, int port, string userName, string password, string displayName, string domain, string webDomain)
        {
            this.hostName       = hostName;
            this.port           = port;
            this.userName       = userName;
            this.password       = password;
            this.displayName    = displayName;
            this.webDomain      = webDomain;

            emailClient = new EmailClient(EMAIL_CLIENT_TYPES.SMTP, hostName, port, userName, password, domain);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="displayName"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="tos"></param>
        /// <param name="ccs"></param>
        /// <param name="bccs"></param>
        /// <param name="encoding"></param>
        /// <param name="clientState"></param>
        /// <returns></returns>
        public Task<bool> Send(string from, string displayName, string subject, string body, bool isBodyHtml, string[] tos, string[] ccs, string[] bccs, Encoding encoding, object clientState)
        {
            try
            {
                if (configOk)
                {
                    //Se inicializa la tarea (Task completetion source)
                    tcs = new TaskCompletionSource<bool>();

                    List<AlternateView> alternateViews = null;
                    if (isBodyHtml) //Si el cuerpo del correo es HTML
                    {
                        if (htmlParser != null && htmlParser.LoadDocument(body)) //Se analiza el documento con en analizador
                        {
                            //Si existen imagenes
                            if (htmlParser.GetHtmlImgTags().Count > 0)
                            {
                                //linkedResources = (from htmlImgTag in htmlParser.GetHtmlImgTags() select new LinkedResource(Path.Combine(currentWorkingDirectory, htmlImgTag.SrcAttribute))).ToList();
                            }
                        }
                        else
                        {
                            //No fue posible cargar el documento

                        }
                    }

                    emailClient.Send(from, displayName, tos, ccs, bccs, subject, body, isBodyHtml, alternateViews, encoding, clientState, this);
                }

                return tcs.Task;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="displayName"></param>
        /// <param name="subject"></param>
        /// <param name="tos"></param>
        /// <param name="ccs"></param>
        /// <param name="bccs"></param>
        /// <param name="templateId"></param>
        /// <param name="templateTypeId"></param>
        /// <param name="enconding"></param>
        /// <param name="clientState"></param>
        /// <returns></returns>
        public Task<bool> Send(string from, string displayName, string subject, string[] tos, string[] ccs, string[] bccs, string templateId, int templateTypeId, Encoding enconding, object clientState)
        {

            try
            {

                if (long.TryParse(templateId, out long longTemplateId))
                {
                    return Send(from, displayName, subject, tos, ccs, bccs, longTemplateId, templateTypeId, enconding, clientState);
                }
                else
                {
                    throw new Exception();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="displayName"></param>
        /// <param name="subject"></param>
        /// <param name="tos"></param>
        /// <param name="ccs"></param>
        /// <param name="bccs"></param>
        /// <param name="templateId"></param>
        /// <param name="enconding"></param>
        /// <param name="clientState"></param>
        /// <returns></returns>
        public Task<bool> Send(string from, string displayName, string subject, string[] tos, string[] ccs, string[] bccs, long templateId, int templateTypeId, Encoding enconding, object clientState)
        {
            try
            {
                //Se inicializa la tarea (Task completetion source)
                tcs = new TaskCompletionSource<bool>();

                //Se obtiene la plantialla
                Template template = templatesManager.GetTemplateByTemplateIdAndType(templateId, (TemplateType)templateTypeId);

                if (template != null)
                {
                    //Si la plantilla es de tipo HTMl
                    switch ((TemplateType)templateTypeId)
                    {
                        
                        case TemplateType.HTML:
                            
                            //Se analiza el documento con en analizador de HTMl
                            if (htmlParser != null && htmlParser.LoadDocument(template.Content))
                            {
                                List<AlternateView> alternateViews = null;

                                //Si existen imagenes en el html
                                if (htmlParser.GetHtmlImgTags().Count > 0)
                                {
                                    string directoryPath = template.DirectoryName;

                                    alternateViews  = new List<AlternateView>();
                                    List<LinkedResource> linkedResources = new List<LinkedResource>();
                                    linkedResources.AddRange(GetLinkedResources(htmlParser.GetHtmlImgTags(), directoryPath, "image/png"));

                                    AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlParser.GetHtml(), null, MediaTypeNames.Text.Html);
                                    //AlternateView alternateView = null;
                                    //Una vez creados los recursos en enlazados se agregan a la vista alternativa
                                    foreach (LinkedResource linkedResource in linkedResources)
                                    {
                                        //AlternateView alternateView = GetEmbeddedImage(linkedResource, htmlParser.GetHtml());
                                        
                                        alternateView.LinkedResources.Add(linkedResource);
                                        
                                    }

                                    alternateViews.Add(alternateView);

                                }

                                emailClient.Send(from, displayName, tos, ccs, bccs, subject, htmlParser.GetHtml(), true, alternateViews, enconding, clientState, this);

                            }
                            else
                            {
                                //No fue posible cargar el documento
                                sendSucess = false;
                                tcs.SetResult(sendSucess);
                            }

                            break;

                    }

                    return tcs.Task;

                }
                else
                {
                    sendSucess = false;
                    tcs.SetResult(sendSucess);
                    return tcs.Task;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlImageObjects"></param>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        private List<LinkedResource> GetLinkedResources(IEnumerable<HtmlImageObject> htmlImageObjects, string directoryPath, string mediaType)
        {
            try
            {
                List<LinkedResource> linkedResources = new List<LinkedResource>();
                
                //Por cada imagen en el HTML se crean los recursos enlazados
                foreach (HtmlImageObject htmlImgTag in htmlImageObjects)
                {
                    LinkedResource linkedResource = new LinkedResource(Path.Combine(directoryPath, htmlImgTag.ImgAttribute), new ContentType("image/png"));
                    linkedResource.ContentType.MediaType = mediaType;
                    linkedResource.ContentType.Name = linkedResource.ContentId;
                    linkedResource.ContentLink = new Uri("cid:" + linkedResource.ContentId);

                    //esta sentencia cambia la propiedad src en el html
                    (htmlImgTag as HtmlImageBehavior).SetImgAttribute(string.Concat("cid:", linkedResource.ContentId));
                    linkedResources.Add(linkedResource);
                }

                return linkedResources;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkedResource"></param>
        /// <param name="htmlImagTag"></param>
        /// <returns></returns>
        private AlternateView GetEmbeddedImage(LinkedResource linkedResource, string htmlImagTag)
        {
            try
            {
                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlImagTag, null, MediaTypeNames.Text.Html);
                alternateView.LinkedResources.Add(linkedResource);
                return alternateView;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="userState"></param>
        /// <param name="success"></param>
        /// <param name="exception"></param>
        public void Completed(object userState, bool success, Exception exception)
        {
           
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailSendResult"></param>
        public void Completed(EmailSendCompleteResult emailSendResult)
        {
            sendSucess = emailSendResult.Success;
            tcs.TrySetResult(sendSucess);

            //Si la operacion de envio no fue exitosa entonces se puede guardar la solicitud de envio y
            //tratar de nuevo mas tarde
            if (!sendSucess)
            {

            }

            //Se restablece la bandera de operacion a falso para la siguiente operacion de envio
            sendSucess = false;
            
        }

      
    }
}