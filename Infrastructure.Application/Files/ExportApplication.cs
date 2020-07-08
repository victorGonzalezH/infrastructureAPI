using Utils.IO.Files.Exports;
using Utils.IO.Files.Exports.Excel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace Infrastructure.Application.Files.Export
{

    
    public interface IExportApplication
    {
        Task<FileDto> Export(ExcelExportCommand excelExportCommand);
        Task<FileDto> ExportExcelAndGetReference(ExcelExportCommand excelExportCommand);
        IFilesApplication FilesApplication { get; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class ExportApplication : IExportApplication
    {

        /// <summary>
        /// 
        /// </summary>
        private IExportsManager exportsManager;


        /// <summary>
        /// 
        /// </summary>
        private IFilesApplication filesApplication;


        public IFilesApplication FilesApplication { get; private set; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="exportsManager"></param>
        public ExportApplication(IExportsManager exportsManager, IFilesApplication filesApplication)
        {
            this.exportsManager = exportsManager;

            this.filesApplication = filesApplication;
            this.FilesApplication = this.filesApplication;
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="excelExportCommand"></param>
        /// <returns></returns>
        private ExcelExportInput ConvertExportExcelCommandToExcelInput(ExcelExportCommand excelExportCommand)
        {

            ExcelSheetInput[] excelSheetInput = (from ExcelSheet excelSheet in excelExportCommand.Sheets select new ExcelSheetInput() { IsRowZeroHeader = excelSheet.IsRowZeroHeader, SheetName = excelSheet.SheetName,  Rows = (from RowCommand rowCommand in excelSheet.RowsCommands select new ExcelRowInput() { ColumnsValues = rowCommand.ColumnsValuesCommands  }).ToArray() }).ToArray();
            return new ExcelExportInput() { FileName = excelExportCommand.FileName, SheetsInputs = excelSheetInput };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excelExportCommand"></param>
        /// <returns></returns>
        public Task<FileDto> Export(ExcelExportCommand excelExportCommand)
        {
            try
            {
                ExcelExportInput excelExportInput = ConvertExportExcelCommandToExcelInput(excelExportCommand);
                Stream stream = this.exportsManager.GenerateExcel(excelExportInput);
                string fileNameWithExtension = string.Concat(excelExportInput.FileName, ".xlsx");
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return Task.FromResult(new FileDto("", fileNameWithExtension, stream, contentType));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }



        public async Task<FileDto> ExportExcelAndGetReference(ExcelExportCommand excelExportCommand)
        {
            try
            {
                ExcelExportInput excelExportInput = ConvertExportExcelCommandToExcelInput(excelExportCommand);
                Stream stream = this.exportsManager.GenerateExcel(excelExportInput);
                string fileNameWithExtension = string.Concat(excelExportInput.FileName, ".xlsx");
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                FileCreation fileCreation = new FileCreation() {  NameWithExtension = fileNameWithExtension, Stream = stream, ContentType = contentType };
                List<FileCreation> filesCreation = new List<FileCreation>();
                filesCreation.Add(fileCreation);
                List<FileDto> filesDto = await this.filesApplication.SaveFilesInMemory(filesCreation);
                FileDto fileDto = null;
                if (filesDto != null && filesDto.Count > 0)
                {
                    fileDto = filesDto[0];             
                }
              
                return fileDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
