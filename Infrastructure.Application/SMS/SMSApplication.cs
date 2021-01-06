
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

using Infrastructure.Application.SMS.UsesCases.SendSMS;
using ApplicationLib.DataTransferObjects;


namespace Infrastructure.Application.SMS
{

    public class SmsApplication: ISmsApplication
    {
        public static readonly string PHONE_NUMBER_WITH_WRONG_FORMAT = "Phone number with wrong format";
        public static readonly int PHONE_NUMBER_WITH_WRONG_FORMAT_CODE = 1;

        /// <summary>
        /// Cliente aws para las notificaciones, (envio de sms)
        /// </summary>
        private IAmazonSimpleNotificationService snsClient;


        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="snsClient"></param>
        public SmsApplication(IAmazonSimpleNotificationService snsClient)
        {
            this.snsClient = snsClient;
        }

        private MessageAttributeValue CreateAttribute(string value)
        {
            MessageAttributeValue attribute = new MessageAttributeValue();
            attribute.DataType = "String";
            attribute.StringValue = value;
            return attribute;
        }


        /// <summary>
        /// Agrega el atributo senderId
        /// </summary>
        /// <param name="messageAttributes">Diccionario de atributos donde se agregan los atributos</param>
        /// <param name="value"></param>
        private void AddSenderIdAttribute(Dictionary<string, MessageAttributeValue> messageAttributes, string value)
        {
            MessageAttributeValue senderIdAttribute = CreateAttribute(value);
            messageAttributes.Add("AWS.SNS.SMS.SenderID", senderIdAttribute);
        }


        /// <summary>
        /// Agrega el atributo tipo de mensaje sms
        /// </summary>
        /// <param name="messageAttributes">Diccionario de atributos donde se agregan los atributos</param>
        /// <param name="smsType">Tipo de mensaje</param>
        private void AddSmsTypeAttribute(Dictionary<string, MessageAttributeValue> messageAttributes, SmsTypes smsType)
        {
            MessageAttributeValue smsTypeAttribute = null;
            if(smsType == SmsTypes.Promotional)
            {
                smsTypeAttribute = CreateAttribute("Promotional");
            }
            else if(smsType == SmsTypes.Transactional)
            {
                smsTypeAttribute = CreateAttribute("Transactional");
            }
            else
            {
                throw new Exception("No a valid SMS Type");
            }
            
            messageAttributes.Add("AWS.SNS.SMS.SMSType", smsTypeAttribute);
        }

        /// <summary>
        /// Verifica si el numero de telefono es correcto
        /// </summary>
        /// <param name="phoneNumber">Numero de telefono, puede estar en formato +1 (nnn) nnn-nnnn o +1nnnnnnnnnn</param>
        /// <returns></returns>
        public bool VerifyPhoneNumberFormat(string phoneNumber)
        {
            // La verificacion debe de hacerse con una expresion regular, esto se implementara en la siguiente version

            if(phoneNumber != null && phoneNumber.Length > 12)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Envia un sms
        /// </summary>
        /// <param name="sendSMSCommand">Comando para enviar un sms</param>
        /// <returns></returns>
        public async Task<ApiResultBase> SendSMS(SendSMSCommand sendSMSCommand)
        {
            try
            {
                return await this.SendSMS(sendSMSCommand.SmsTpe, sendSMSCommand.SenderId, sendSMSCommand.Message, sendSMSCommand.PhoneNumber);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Envia mensaje sms
        /// </summary>
        /// <param name="smsType">Tipo de mensaje sms</param>
        /// <param name="senderId">Identificador del enviador, puede ser una cadena</param>
        /// <param name="message">Mensaje</param>
        /// <param name="phoneNumber">Numero de telefono</param>
        /// <returns></returns>        
        public async Task<ApiResultBase> SendSMS(SmsTypes smsType, string senderId, string message, string phoneNumber)
        {
            try
            {
                if(this.VerifyPhoneNumberFormat(phoneNumber))
                {   
                    //Instancia el diccionario de los atributos
                    Dictionary<string, MessageAttributeValue> messageAttributes = new Dictionary<string, MessageAttributeValue>();
                    //Se agrega el atributo SenderId
                    this.AddSenderIdAttribute(messageAttributes, senderId);
                    //Se agrega el atributo tipo de mensaje
                    this.AddSmsTypeAttribute(messageAttributes, smsType);
                    //Se envia el mensaje SMS
                    SendSMSResultDto sendSMSResultDto = await this.SendSMSMessageAsync(message, phoneNumber, messageAttributes);
                    return new ApiResultBase(){ IsSuccess = true, Data = sendSMSResultDto, ResultCode = (int)ApiResultCodes.SUCESS, ApplicationMessage = ApiResultBase.SUCCESS };

                }
                else
                {
                    return new ApiResultBase(){ 
                    IsSuccess = false,
                    Data = null,
                    Error = new PhoneNumberException(PHONE_NUMBER_WITH_WRONG_FORMAT),
                    ResultCode = PHONE_NUMBER_WITH_WRONG_FORMAT_CODE,
                    ApplicationMessage = PHONE_NUMBER_WITH_WRONG_FORMAT };

                }
                
            }
            catch(Exception ex)
            {
                return new ApiResultBase(){ 
                    IsSuccess = false,
                    Data = null,
                    Error = ex,
                    ResultCode = (int)ApiResultCodes.ERROR,
                    ApplicationMessage = ApiResultBase.ERROR };
            }
        }


        /// <summary>
        /// Envia un mensaje sms
        /// </summary>
        /// <param name="message">Mensaje a enviar</param>
        /// <param name="phoneNumber">Numero de telefono, debe estar en formato (+country Code)nnnnnnnnnn</param>
        /// <param name="messageAttributes">Atributos para enviar sms</param>
        /// <returns></returns>
        private async Task<SendSMSResultDto> SendSMSMessageAsync(string message, string phoneNumber, Dictionary<string, MessageAttributeValue> messageAttributes)
        {
            PublishRequest publishRequest       = new PublishRequest();
            publishRequest.PhoneNumber          = phoneNumber;
            publishRequest.Message              = message;
            publishRequest.MessageAttributes    = messageAttributes;
            try
            {
                PublishResponse response = await snsClient.PublishAsync(publishRequest);
                return new SendSMSResultDto(response.MessageId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

    }

}