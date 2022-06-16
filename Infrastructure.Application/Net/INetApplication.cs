using ApplicationLib.Microservices;
using Infrastructure.Application.Net.UseCases.RunTcpServer;

namespace Infrastructure.Application.Net
{

    public interface INetApplication
    {
        int RunTcpServer(IMicroserviceClient client, int port, string forwardHost, int forwardPort = 0, ForwardMessageType messageType = ForwardMessageType.String);

        int RunTcpServer(IMicroserviceClient client, RunTcpServerCommand runTcpServerCommand);

    }

}