namespace Infrastructure.Application.Email 
{
    public class EmailSettings
    {
        public string HostName { get; set; }

        public int Port { get; set; }

        public string Domain { get; set; }

        public string WebDomain { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public bool EnableSsl {get; set;}
    }
}