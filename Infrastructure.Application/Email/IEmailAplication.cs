using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.Email
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEmailApplication
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        void Config(string host, int port, string userName, string password, string domain, bool enableSsl);

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
        Task<bool> Send(string from, string displayName, string subject, string body, bool isBodyHtml, string[] tos, string[] ccs, string[] bccs, Encoding encoding, object clientState);



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
        Task<bool> Send(string from, string displayName, string subject, string[] tos, string[] ccs, string[] bccs, long templateId, int templateTypeId, Encoding enconding, object clientState);



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
        Task<bool> Send(string from, string displayName, string subject, string[] tos, string[] ccs, string[] bccs, string templateId, int templateTypeId, Encoding enconding, object clientState);
    }
}