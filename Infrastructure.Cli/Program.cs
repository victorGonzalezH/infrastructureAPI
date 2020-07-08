using System;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Domain.Files;

namespace Infrastructure.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            InfrastructureContext context = new InfrastructureContext(null);
            var filesCollection = context.Set<File>();
            Console.ReadKey();
        }
    }
}
