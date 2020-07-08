using ApplicationLib.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Application.Images
{


    public class DeleteImageResult: DtoBase
    {


        /// <summary>
        /// Especifica los ids de las imagenes que se eliminaron
        /// </summary>
        public List<string> DeleteImagesIds { get; set; }


        /// <summary>
        /// Especifica los ids de los imagenes que no se eliminaron
        /// </summary>
        public List<string> NoDeleteImagesIds { get; set; }


        
        /// <summary>
        /// DTO para el resultado de la operacion de eliminar imagenes
        /// </summary>
        public DeleteImageResult()
        {

            DeleteImagesIds = new List<string>();

            NoDeleteImagesIds = new List<string>();

        }

    }
}
