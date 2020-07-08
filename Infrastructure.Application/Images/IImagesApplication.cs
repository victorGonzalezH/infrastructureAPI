using Utils.IO.Images;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Application.Images
{
    public interface IImagesApplication
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addTextToImageCommand"></param>
        /// <returns></returns>
        AddTextToImageResultDto AddTextToImage(AddTextToImageCommand addTextToImageCommand);


        /// <summary>
        /// Agrega una
        /// </summary>
        /// <param name="addTextToImageCommand"></param>
        /// <returns></returns>
        AddTextToImageResultDto AddTextsToImage(AddTextsToImageCommand addTextToImageCommand);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteImage(string ids);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        DeleteImageResult DeleteImages(string[] ids);



    }
}
