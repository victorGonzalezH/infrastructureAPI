using SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInfrastructureUnitOfWork : IUnitOfWork
    {

        IRepository<Infrastructure.Domain.Files.EntityConfiguration> EntitiesConfigurationsRepository { get; }


        IRepository<Infrastructure.Domain.Files.File> FilesRepository { get; }

    }
}
