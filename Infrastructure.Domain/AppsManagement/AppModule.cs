using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Domain.AppsManagement
{

    public class AppModule
    {

        /// <summary>
        /// Identificador unico del modulo
        /// </summary>
        public int AppModuleId { get; set; }


        /// <summary>
        /// Codigo nombre del AppModule
        /// </summary>
        public string CodeName { get; set; }


        /// <summary>
        /// Texto para mostrar en una vista de catalogo
        /// </summary>
        public string Display { get; set; }

        
        /// <summary>
        /// Descripcion del module
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Secuencia de ordenamiento para una vista en catalogo
        /// </summary>
        public int Sequence { get; set; }


        /// <summary>
        /// Si el AppModule es visible o no en una vista de catalogo
        /// </summary>
        public bool Visible { get; set; }


        /// <summary>
        /// Si el AppModule esta habilitado o no
        /// </summary>
        public bool Enabled { get; set; }


        /// <summary>
        /// AppModule padre
        /// </summary>
        public string ParentId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int DefaultModuleRoleId { get; set; }


        /// <summary>
        /// Roles del modulo
        /// </summary>
        public List<ModuleRol> ModulesRoles { get; set; }

    }
}
