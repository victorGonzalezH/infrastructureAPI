﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


using ApplicationLib.DataTransferObjects;
using WebApplication.Controllers;
using Infrastructure.Application.Email;
using Infrastructure.API.Application.Email.UseCases.SendEmail;

namespace Infrastructure.API.EmailService.Controllers
{
    [Route("api/[controller]")]
    public class EmailsController: ApiBaseController
    {

        /// <summary>
        /// 
        /// </summary>
        private EmailSettings emailSettings;

      

        /// <summary>
        /// 
        /// </summary>
        private IEmailApplication emailApplication;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        public EmailsController(IOptionsSnapshot<EmailSettings> emailSettings, IEmailApplication emailApplication)
        {

            //Asignacion de la configuracion del correo electronico
            this.emailSettings = emailSettings.Value;

            //Aplicacion de correo
            this.emailApplication = emailApplication;

        }


        [Route("send")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SendEmailResultDto>> Send(SendEmailCommand emailData)
        {

            //Primera validacion a nivel web
            if (emailData.Subject == null || emailData.Tos == null)
            {
                return BadRequest();
            }
            else
            {

                try
                {
                    if (emailData.Tos.Length == 0)
                    {
                        return BadRequest();
                    }


                    if ( (emailData.TemplateId == null || emailData.TemplateId == "") && emailData.Body == null)
                    {
                        return BadRequest();
                    }
                 
                    //Se configura la aplicacion de correo
                    emailApplication.Config(emailSettings.HostName, emailSettings.Port, emailSettings.UserName, emailSettings.Password, emailSettings.Domain, emailSettings.EnableSsl);

                    string from     = emailSettings.UserName + emailSettings.WebDomain;
                    string[] ccs    = emailData.Ccs == null ? new string[] { } : emailData.Ccs;
                    string[] bccs   = emailData.Bccs == null ? new string[] { } : emailData.Bccs;
                    string[] tos    = emailData.Tos;

                    bool succes = false;

                    //Si el mensaje de correo contiene el identificador valido de una plantilla
                    if (emailData.TemplateId != null && emailData.TemplateId != "")
                    {
                        succes = await emailApplication.Send(from, emailSettings.DisplayName, emailData.Subject, tos, ccs, bccs, emailData.TemplateId, emailData.TemplateTypeId, Encoding.UTF8, emailData.Id);
                    }
                    else
                    {
                        succes = await emailApplication.Send(from, emailSettings.DisplayName, emailData.Subject, emailData.Body, emailData.IsBodyHtml, emailData.Tos, ccs, bccs, Encoding.UTF8, emailData.Id);
                    }
                    


                    return succes ? Ok(new ApiResultBase() { IsSuccess = true, Data = new SendEmailResultDto(emailData.Id) {  }, Token = 0 } ) : Ok(new ApiResultBase() { IsSuccess = false });

                }
                catch (Exception ex)
                {
                    //Log ex;
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }


    }
}