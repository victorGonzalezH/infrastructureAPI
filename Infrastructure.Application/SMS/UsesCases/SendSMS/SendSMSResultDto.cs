
namespace Infrastructure.Application.SMS.UsesCases.SendSMS
{

    public class SendSMSResultDto
    {

        /// <summary>
        /// Identificador unico
        /// </summary>
        /// <value></value>
        public string Id {get; set;}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public SendSMSResultDto(string id)
        {
            this.Id =  id;
        }

    }

}