using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.API.Files
{


    /// <summary>
    /// 
    /// </summary>
    public class FileApiSettings
    {

        public string Domain { get; set; }

        public string Protocol { get; set; }

        public int Port { get; set; }

        public List<EntityUrlPath> EntitiesUrlPaths { get; set; }

    }


    /// <summary>
    /// 
    /// </summary>
    public class EntityUrlPath
    {
        /// <summary>
        /// 
        /// </summary>
        public string EntityCodeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UrlPath { get; set; }

    }

}
