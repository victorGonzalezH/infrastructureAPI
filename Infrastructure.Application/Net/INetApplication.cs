using ApplicationLib.Microservices;
using Infrastructure.Application.Net.UseCases.RunTcpServer;

namespace Infrastructure.Application.Net
{

    public interface INetApplication
    {
        bool RunTcpServer(IMicroserviceClient client, int port, string forwardHost, int forwardPort = 0, ForwardMessageType messageType = ForwardMessageType.String);

        bool RunTcpServer(IMicroserviceClient client, RunTcpServerCommand runTcpServerCommand);

    }

}