using ApplicationLib.Microservices;

namespace Infrastructure.Application.Net
{

    public interface INetApplication
    {
        void RunTcpServer(IMicroserviceClient client, int port, string forwardHost, int forwardPort = 0, ForwardMessageType messageType = ForwardMessageType.String);

    }

}