using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Domain.AppsManagement
{


    public interface IAppsModulesRepository
    {



        IEnumerable<AppModule> GetAppModuleWithModuleRoles();



    }
}
