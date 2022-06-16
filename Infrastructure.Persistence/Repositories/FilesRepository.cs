using Infrastructure.Domain.Files;
using Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class FilesRepository : Repository<File>, IFilesRepository
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public FilesRepository(InfrastructureContext context) : base(context)
        {
                

        }



    }
}
