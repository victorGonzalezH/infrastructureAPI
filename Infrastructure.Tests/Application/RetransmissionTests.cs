using System;
using Xunit;
using System.Threading.Tasks;
using Infrastructure.Application.Retransmission;

namespace Infrastructure.Tests.Application
{
    public class RetransmissionApplicationTest
    {

        /// <summary>
        /// Verifica que se envie un mensaje sms correctamente
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Get_Token_Sucess()
        {
            IRetransmissionApplication retransmissionApplication = new RetransmissionApplication();
            
            //When
            var result = await retransmissionApplication.GetUserToken("wm_10405_SERTICOMNETWORKS", "baWo_951zEaw*0");

            //Then
            Assert.NotNull(result.token);

        }

    }

}