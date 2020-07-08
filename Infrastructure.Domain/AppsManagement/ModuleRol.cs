using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Domain.AppsManagement
{


    public class ModuleRol
    {

        public int ModuleRolId { get; set; }


        public int AppModuleId { get; set; }


        public string CodeName { get; set; }


        public string Display { get; set; }


        public string Description { get; set; }


        public bool List { get; set; }


        public bool Read { get; set; }


        public bool Write { get; set; }


        public bool Modify { get; set; }


        public bool Delete { get; set; }
        

        public bool Approve { get; set; }


        public string CustomPermission { get; set; }


        public List<RolUser> RolesUsers { get; set; }

    }
}
