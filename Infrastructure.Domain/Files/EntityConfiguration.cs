using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Domain.Files
{

    /// <summary>
    /// 
    /// </summary>
    public class EntityConfiguration
    {

        public int Id { get; set; }

        public string EntityCodeName { get; set; }

        public string Description { get; set; }

        public string Server { get; set; }

        public string Path { get; set; }

        public string FolderCreationExpression { get; set; }

        public bool IsRelativePath { get; set; }

        public int Sequence { get; set; }

        public bool Visible { get; set; }

        public bool Enabled { get; set; }

        public List<File> Files { get; set; }
    }
}
