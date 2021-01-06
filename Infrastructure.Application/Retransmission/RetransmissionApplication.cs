using ServiceReference;
using System.Threading.Tasks;


namespace Infrastructure.Application.Retransmission
{
    public class RetransmissionApplication: IRetransmissionApplication
    {

        /// <summary>
        /// Cliente WCF
        /// </summary>
        private RCServiceClient client;

        /// <summary>
        /// Constructor
        /// </summary>
        public RetransmissionApplication()
        {
            this.client = new RCServiceClient();
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<UserTokenResult> GetUserToken(string userId, string password)
        {
            return await this.client.GetUserTokenAsync(userId, password);
        }



        public void Retransmit()
        {
            
        }

    }

}