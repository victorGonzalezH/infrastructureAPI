namespace Infrastructure.Application.Net.UseCases.RunTcpServer
{
    public class TcpServerConfig
    {

        /// <summary>
        /// Name or ip address where tcp servers that are going to be created are going to listen to
        /// You have to set this Ip or name according to the environment where is running
        /// </summary>
        /// <value></value>
        public string HostNameOrIp { get; set; }
    }
}