using Utils.IO.Images;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Infrastructure.Application.Images
{


    public class ImagesApplication : IImagesApplication
    {

        private IImagesManager imagesManager;


        public ImagesApplication(IImagesManager imagesManager)
        {
            this.imagesManager = imagesManager;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long UtcNowTicks()
        {
            long lastTimeStamp = DateTime.UtcNow.Ticks;
            long original, newValue;
            do
            {
                original = lastTimeStamp;
                long now = DateTime.UtcNow.Ticks;
                newValue = Math.Max(now, original + 1);
            } while (Interlocked.CompareExchange(ref lastTimeStamp, newValue, original) != original);

            return newValue;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="addTextToImageCommand"></param>
        /// <returns></returns>
        public AddTextToImageResultDto AddTextToImage(AddTextToImageCommand addTextToImageCommand)
        {

            if (addTextToImageCommand != null)
            {
                string imageName = addTextToImageCommand.ImageName;
                string text     = addTextToImageCommand.Text;
                float pointFX = 0f;
                float pointFY = 0f;
                if (float.TryParse(addTextToImageCommand.PointFX, out pointFX) && float.TryParse(addTextToImageCommand.PointFY, out pointFY))
                {
                    System.Drawing.PointF pointF = new System.Drawing.PointF(pointFX, pointFY);

                    ImageUtil imageUtil = imagesManager.GetImage(imageName);

                    if (imageUtil != null)
                    {
                        using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(imageUtil.Bitmap))
                        {
                            using (System.Drawing.Font font = new System.Drawing.Font(addTextToImageCommand.Font, addTextToImageCommand.FontSize, (System.Drawing.FontStyle)addTextToImageCommand.FontStyle))
                            {
                                graphics.DrawString(text, font, System.Drawing.Brushes.White, pointF);
                            }
                        }

                        string id = UtcNowTicks().ToString();

                        string newPath = System.IO.Path.Combine(imagesManager.GetOutputDirectory(), string.Concat(id, imageUtil.Extension));
                        imageUtil.Bitmap.Save(newPath);

                        return new AddTextToImageResultDto() { ImageName = System.IO.Path.GetFileName(newPath) };
                    }
                    else
                    {
                        return null;
                    }
                    
                }
                else
                {
                    return null;
                }
                
            }

            return null;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="text"></param>
        /// <param name="pointFXParam"></param>
        /// <param name="pointFYParam"></param>
        /// <param name="font"></param>
        /// <param name="fontStyle"></param>
        /// <param name="fontSize"></param>
        private bool DrawString(Graphics graphics, string text, string pointFXParam, string pointFYParam, string font, int fontStyle, int fontSize)
        {
            if (text != null && text != "")
            {
                float pointFX = 0f;
                float pointFY = 0f;
                if (float.TryParse(pointFXParam, out pointFX) && float.TryParse(pointFYParam, out pointFY))
                {
                    PointF pointF = new System.Drawing.PointF(pointFX, pointFY);
                    using (Font Font = new Font(font, fontSize, (FontStyle)fontStyle))
                    {
                        graphics.DrawString(text, Font, System.Drawing.Brushes.White, pointF);
                    }

                    return true;
                }

            }

            return false;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="addTextsToImageCommand"></param>
        /// <returns></returns>
        public AddTextToImageResultDto AddTextsToImage(AddTextsToImageCommand addTextsToImageCommand)
        {
            try
            {

                if (addTextsToImageCommand != null && addTextsToImageCommand.Texts != null)
                {
                    int successCounter = 0;
                    string imageName = addTextsToImageCommand.ImageName;
                    ImageUtil imageUtil = imagesManager.GetImage(imageName);

                    using (Graphics graphics = Graphics.FromImage(imageUtil.Bitmap))
                    {
                        foreach (TextCommand textCommand in addTextsToImageCommand.Texts)
                        {
                            if (DrawString(graphics, textCommand.Text, textCommand.PointFX, textCommand.PointFY, textCommand.Font, textCommand.FontStyle, textCommand.FontSize))
                            {
                                successCounter++;
                            }

                        }
                    }

                    if (successCounter == addTextsToImageCommand.Texts.Count)
                    {
                        string id = UtcNowTicks().ToString();
                        string newPath = System.IO.Path.Combine(imagesManager.GetOutputDirectory(), string.Concat(id, imageUtil.Extension));
                        imageUtil.Bitmap.Save(newPath);
                        return new AddTextToImageResultDto() { ImageName = System.IO.Path.GetFileName(newPath) };
                    }

                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteImage(string id)
        {

            try
            {
                ImageUtil image = imagesManager.GetImage(id);
                if (image != null)
                {
                    imagesManager.Delete(image);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public DeleteImageResult DeleteImages(string[] ids)
        {

            try
            {
                DeleteImageResult deleteImageResult = new DeleteImageResult();
                foreach (string id in ids)
                {
                    if (DeleteImage(id))
                    {
                        deleteImageResult.DeleteImagesIds.Add(id);
                    }
                    else
                    {
                        deleteImageResult.NoDeleteImagesIds.Add(id);
                    }
                }

                return deleteImageResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
