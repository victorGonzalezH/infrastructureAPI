using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Application.Images
{


    /// <summary>
    /// 
    /// </summary>
    public class AddTextToImageCommand
    {

        public string ImageName { get; set; }


        public string Text { get; set; }


        public string PointFX { get; set; }


        public string PointFY { get; set; }


        public string Font { get; set; }


        public int FontSize { get; set; }


        public int FontStyle { get; set; }

    }



    /// <summary>
    /// 
    /// </summary>
    public class AddTextsToImageCommand
    {
        public string ImageName { get; set; }

        public List<TextCommand> Texts;
    }


    /// <summary>
    /// 
    /// </summary>
    public class TextCommand
    {

        
        public string Text { get; set; }


        public string PointFX { get; set; }


        public string PointFY { get; set; }


        public string Font { get; set; }


        public int FontSize { get; set; }


        public int FontStyle { get; set; }

    }

}
