
using System.Threading.Tasks;
using ApplicationLib.DataTransferObjects;
using Infrastructure.Application.SMS.UsesCases.SendSMS;

namespace Infrastructure.Application.SMS
{
    public interface ISmsApplication
    {

        /// <summary>
        /// Envia un mensaje sms
        /// </summary>
        /// <param name="smsType"></param>
        /// <param name="senderId"></param>
        /// <param name="message"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<ApiResultBase> SendSMS(SmsTypes smsType, string senderId, string message, string phoneNumber);

        
        /// <summary>
        /// Envia un mensaje sms
        /// </summary>
        /// <param name="sendSMSCommand"></param>
        /// <returns></returns>
        Task<ApiResultBase> SendSMS(SendSMSCommand sendSMSCommand);
        

        /// <summary>
        /// Verifica que el numero de telefono tenga el formato correcto
        /// </summary>
        /// <param name="phoneNumber">Numero de telefono, puede estar en formato +1 (nnn) nnn-nnnn o +1nnnnnnnnnn</param>
        /// <returns></returns>
        bool VerifyPhoneNumberFormat(string phoneNumber);

    }
}