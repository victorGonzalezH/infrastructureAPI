using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Domain.Files
{

    /// <summary>
    /// 
    /// </summary>
    public class File
    {

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Identificador de la entidad a la que pertenece el archivo, puede ser posible que el 
        /// archivo no pertenezca ninguna entidad
        /// </summary>
        public int? EntityConfigurationId { get; set; }

        /// <summary>
        /// Ruta completa donde se encuentra el archivo, (sin nombre y sin extension)
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 
        /// </summary>
        private string extension;


        /// <summary>
        /// Extension del archivo con punto (.)
        /// </summary>
        public string Extension
        {

            get { return this.extension; }

            set {
                if (!value.Contains(".")) this.extension = "." + value;
                else this.extension = value;
            }
        }
       
        

        /// <summary>
        /// 
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityConfiguration EntityConfiguration { get; set; }


        /// <summary>
        /// Fecha de creacion
        /// </summary>
        public DateTime? CreationDate { get; set; }


        public string NameWithExtension { get { return Name + Extension; } }


        public string PathWithNameWithExtension { get { return Path + Name + Extension; } }

    }


}
