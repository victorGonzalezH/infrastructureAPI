using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Infrastructure.Application.Files.Export
{
    public class ExportDto
    {

        /// <summary>
        /// 
        /// </summary>
        public string MIMEType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileNameWithExtension { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileNameWithExtension"></param>
        /// <param name="Content"></param>
        public ExportDto(string fileNameWithExtension, Stream stream, string MIMEType)
        {
            this.FileNameWithExtension = fileNameWithExtension;
            this.Stream = stream;
            this.MIMEType = MIMEType;
        }
        
    }
}
