using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Infrastructure.Application.Files;
using Infrastructure.Application.Files.Export;
using WebApplication.Controllers;
using ApplicationLib.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Infrastructure.API.Files.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportsController : ApiBaseController
    {

        /// <summary>
        /// 
        /// </summary>
        private IExportApplication exportApplication;


        /// <summary>
        /// Configuracion de la Interfaz de Programacion de Aplicaciones de Archivos
        /// </summary>
        private FileApiSettings fileApiSettings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exportApplication"></param>
        public ExportsController(IExportApplication exportApplication, IOptionsSnapshot<FileApiSettings> settings)
        {
            this.exportApplication = exportApplication;
            fileApiSettings = settings.Value;
        }



        [Route("ExportExcel")]
        [HttpPost]
        public async Task<IActionResult> ExportExcel([FromBody]ExcelExportCommand exportCommand)
        {
            FileDto fileDto = await exportApplication.Export(exportCommand);
            return File(fileDto.Stream, fileDto.ContentType, fileDto.FullName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="exportCommand"></param>
        /// <returns></returns>
        [Route("ExportExcelAndGetReference")]
        [HttpPost]
        public async Task<IActionResult> ExportExcelAndGetReference([FromBody]ExcelExportCommand exportCommand)
        {
            this.exportApplication.FilesApplication.Config(fileApiSettings.Protocol, fileApiSettings.Domain, fileApiSettings.Port, "resources/files");
            FileDto fileDto = await exportApplication.ExportExcelAndGetReference(exportCommand);
            return Ok(new ApiResultBase() { IsSuccess = true, Data = fileDto, Error = null });

        }

    }
}