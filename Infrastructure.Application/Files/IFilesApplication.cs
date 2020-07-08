using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.Files
{
    public interface IFilesApplication
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityCodeName"></param>
        /// <param name="streams"></param>
        Task<List<FileDto>> SaveFiles(string entityCodeName, List<FileCreation> filesCreations);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<FileDto> GetFileAsync(string id);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        string GetFileUrl(string uid);


        /// <summary>
        /// Establece la configuracion de la aplicacion
        /// </summary>
        /// <param name="protocol">Protocolo bajo el cual opera la aplicacion de archivos</param>
        /// <param name="domain">Dominio en el cual opera la aplicacion</param>
        /// <param name="port">Puerto en el que opera la aplicacion</param>
        /// <param name="urlPath">Ruta en la que opera la aplicacion</param>
        void Config(string protocol, string domain, int port, string urlPath);


        /// <summary>
        /// 
        /// </summary>
        bool IsReady { get; }


        Task<List<FileDto>> SaveFilesInMemory(List<FileCreation> filesCreations);
    }
}
