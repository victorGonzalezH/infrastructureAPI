using ServiceReference;
using System.Threading.Tasks;

namespace Infrastructure.Application.Retransmission
{

    public interface IRetransmissionApplication
    {

        /// <summary>
        /// Obtiene un token
        /// </summary>
        /// <param name="userId">Id del usuario</param>
        /// <param name="password">Contrasena</param>
        /// <returns></returns>
        Task<UserTokenResult> GetUserToken(string userId, string password);

        // void Retransmit();

    }

}