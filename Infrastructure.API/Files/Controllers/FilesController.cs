using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Application.Files.UseCases.UploadFile;
using Infrastructure.Application.Files;
using WebApplication.Controllers;
using ApplicationLib.DataTransferObjects;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SeedWork;

namespace Infrastructure.API.Files
{

    [Route("resources/[controller]")]
    [ApiController]
    public class FilesController : ApiBaseController
    {

        /// <summary>
        /// Clase de 
        /// </summary>
        private IFilesApplication filesApplication;


        /// <summary>
        /// Configuracion de la Interfaz de Programacion de Aplicaciones de Archivos
        /// </summary>
        private FileApiSettings fileApiSettings;



        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public FilesController(IFilesApplication filesApplication, IOptionsSnapshot<FileApiSettings> settings)
        {
            
            this.filesApplication = filesApplication;

            fileApiSettings       = settings.Value;

            if (!filesApplication.IsReady)
            {
                filesApplication.Config(fileApiSettings.Protocol, fileApiSettings.Domain, fileApiSettings.Port, "resources/files");
            }
        }


        // [EnableCors("CorsPolicy")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Download(string id, [FromQuery]string entityCodeName, [FromQuery] int storageLocation)
        {
            try
            {
                if (id == null || id == "")
                {
                    return BadRequest();
                }
                else
                {
                    FileDto fileDto = await filesApplication.GetFileAsync(id);
                    if (fileDto != null)
                    {
                        if (fileDto.ContentType == null || fileDto.ContentType == "")
                        {
                            return File(fileDto.Stream, "application/octet-stream", fileDto.FullName);
                        }
                        else
                        {
                            return File(fileDto.Stream, fileDto.ContentType, fileDto.FullName);
                        }
                    }
                        

                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                //Log ex

                return NotFound();
            }
        }




        



        [Route("Upload")]
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] UploadFilesCommand uploadFilesCommand)
        {
            try
            {
                
                if (uploadFilesCommand.Files != null && uploadFilesCommand.Files.Count > 0)
                {
                    List<IFormFile> files = uploadFilesCommand.Files;
                    long size = files.Sum(f => f.Length);

                    //Se abren los streams por cada archivo que contenga el comando


                    //RECORDAR CERRAR TODOS LOS STREAMS
                    //NO CONFIAR EN EL NOMBRE DEL ARCHIVO, SE TIENE QUE HACER LAS VALIDACIONES NECESARIAS
                    List<FileCreation> filesCreations = (from file in uploadFilesCommand.Files
                                                            select new FileCreation()
                                                            {
                                                                Stream = file.OpenReadStream(),
                                                                NameWithExtension = file.FileName //<-- La validacion para file.Name se hace en la clase FileCreation

                                                            }).ToList();


                    //Guardando los archivos
                    List<FileDto> filesDtos = await filesApplication.SaveFiles(uploadFilesCommand.EntityCodeName, filesCreations);

                    return Ok(new ApiResultBase() { IsSuccess = true, Data = filesDtos.Where(fileDto => fileDto.Url != null), Error = null });
                }
                else
                {
                    return BadRequest();
                }
            
            }
            catch (Exception ex)
            {
                //Log ex
                return Ok(new ApiResultBase() { IsSuccess = false, Data = null, ResultCode = -1 });
            }

        }



    }
}