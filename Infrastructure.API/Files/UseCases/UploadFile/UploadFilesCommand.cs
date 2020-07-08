using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Application.Files.UseCases.UploadFile
{
    /// <summary>
    /// Comando para guardar los archivos. Las propiedades EntityCodeName y EntityId se usan
    /// para saber que entidad u objeto de negocio es dueno de los archivos que se envian. La
    /// Propiedad entityId es para saber que identificador tiene ese objeto en la tabla donde
    /// se almacena. Por ejemplo la propiedad EntityCodeName puede tener el valor de REQUEST y
    /// la propiedad EntityId puede tener el valor de 234. Entonces esto quiere decir que los
    /// documentos enviados corresponden a un registro de la tabla ToolRequests (La propiedad
    /// EntityCodeName no necesariamente es el mismo al nombre de la tabla en el que se guardan)
    /// con identificador 234.
    /// </summary>
    public class UploadFilesCommand
    {
        /// <summary>
        /// Codigo del nombre de la entidad a la que pertenecen los archivos.
        /// </summary>
        public string EntityCodeName { get; set; }


        /// <summary>
        /// Identificador del registro del tipo de entidad a la que pertenecen los archivos
        /// </summary>
        public int EntityId { get; set; }


        /// <summary>
        /// Listado de archivos
        /// </summary>
        public List<IFormFile> Files { get; set; }



    }
}
