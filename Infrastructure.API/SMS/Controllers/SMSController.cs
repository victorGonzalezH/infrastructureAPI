using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApplicationLib.Microservices.Controllers;
using ApplicationLib.Microservices.Messages;
using Infrastructure.Application.SMS;
using Infrastructure.Application.SMS.UsesCases.SendSMS;
using ApplicationLib.DataTransferObjects;


namespace Infrastructure.API.SMS.Controllers
{

    [MicroserviceController("sms")]
    [Route("api/[controller]")]
    public class SmsController: MicroserviceControllerBase
    {
    
        /// <summary>
        /// Aplicacion SMS
        /// </summary>
        private ISmsApplication smsApplication;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smsApplication"></param>
        public SmsController(ISmsApplication smsApplication)
        {
            this.smsApplication = smsApplication;    
        }


        ///<summary>
        ///Envia un mensaje sms
        ///</summary>
        public async Task<object> MicroserviceSend(MicroserviceMessage message)
        {
            try
            {
                //Este paso de deserializacion se tiene que hacer en el controlador (aqui) por que en la clase
                //StartupCommunication se desconoce el tipo de dato con que se desarializara en este caso
                //SendEmailCommand
                SendSMSCommand sendSMSCommand =  message.DeserializeParamValues<SendSMSCommand>();
                ApiResultBase result = await this.smsApplication.SendSMS(sendSMSCommand);
                return result;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }

}