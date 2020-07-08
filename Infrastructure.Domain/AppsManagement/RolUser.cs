using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Domain.AppsManagement
{
    public class RolUser
    {

        /// <summary>
        /// Identificador del ModuleRol
        /// </summary>
        public int ModuleRolId { get; set; }



        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public int UserId { get; set; }



        /// <summary>
        /// Module Rol
        /// </summary>
        public ModuleRol ModuleRol { get; set; }

    }
}
