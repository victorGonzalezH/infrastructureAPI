using Infrastructure.Domain.AppsManagement;
using Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{


    /// <summary>
    /// 
    /// </summary>
    public class AppsModulesRepository : Repository<AppModule>, IAppsModulesRepository
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public AppsModulesRepository(InfrastructureContext context) : base(context)
        {

        }

        public IEnumerable<AppModule> GetAppModuleWithModuleRoles()
        {
            throw new NotImplementedException();
        }
    }
}
