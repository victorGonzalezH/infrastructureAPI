using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Application.Files.Export
{

    /// <summary>
    /// Comando para exportar a excel
    /// </summary>
    public class ExcelExportCommand
    {
        /// <summary>
        /// Nombre del archivo a exportar
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Nombres de las hojas
        /// </summary>
        public ExcelSheet[] Sheets { get; set; }
        
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExcelSheet
    {
        /// <summary>
        /// Nombre de la hoja
        /// </summary>
        public string SheetName { get; set; }


        /// <summary>
        /// Indica si la fila cero se toma como encabezado
        /// </summary>
        public bool IsRowZeroHeader { get; set; }


        /// <summary>
        /// Arreglo de filas
        /// </summary>
        public RowCommand[] RowsCommands { get; set; }

    }


    /// <summary>
    /// 
    /// </summary>
    public class RowCommand
    {
       
        /// <summary>
        /// Columnas de la fila
        /// </summary>
        public string[] ColumnsValuesCommands { get; set; }
        
    }
    
}
