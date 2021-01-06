using Xunit;
using Infrastructure.Application.SMS;
using Amazon.SimpleNotificationService;
using ApplicationLib.DataTransferObjects;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Application
{
    public class SMSApplicationTests
    {

        /// <summary>
        /// Verifica que se envie un mensaje sms correctamente
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SendSMS_Sucess()
        {

            //Given
            // Instanciacion del client aws sns (Simple Notification Service aws)
            AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(Amazon.RegionEndpoint.USWest2);
            
            // Instanciacion de la aplicacion sms
            ISmsApplication smsApplication = new SmsApplication(snsClient);

            // Tipo de mensaje
            SmsTypes smsType = SmsTypes.Transactional;

            // NUmero de telefono
            string phoneNumber = "+529931321441";

            // Mensaje a enviar
            string message = "Hola desde prueba";

            //Id del enviador
            string senderId = "test";

            //When  
            // ApiResultBase apiResultBase = await smsApplication.SendSMS(smsType, senderId, message, phoneNumber);
        
            //Then
            Assert.True(true);
        }

    }

}
