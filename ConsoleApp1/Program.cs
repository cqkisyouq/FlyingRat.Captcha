using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp.Drawing.Processing;
using FlyingRat.Captcha;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            //TestImage();
        }

        public static  void Test()
        {
            using Image<Rgba32> image = Image.Load<Rgba32>("Image/base/背景10.jpg");
            using var alpha = Image.Load<Rgba32>("Image/base/alpha.png");
            using var top = Image.Load<Rgba32>("Image/base/top.png");
            using var slider = Image.Load<Rgba32>("Image/base/base.png");
            Random rd = new Random(DateTime.Now.Millisecond);
            var x = rd.Next(0, image.Width - top.Width);
            var y = rd.Next(0, image.Height - top.Height);
            var point = new Point(x, y);
            var handler = new ImageDriver();
            handler.CopyNoAlpha(image, point, alpha);
            //handler.Copy(image, point, slider);
            alpha.Mutate(x =>
            {
                x.DrawImage(top, 1);
            });
            image.Mutate(x =>
            {
                x.DrawImage(slider, point, 1);
            });
            var col = 15;
            var row = 5;
            var imgData = handler.RandImagesBy(image,ref col,ref row);
            var newImage = new Image<Rgba32>(image.Width, image.Height);
            using var test =handler.ImageByRandImages(newImage, imgData, col);
            //for (int i = 0; i < imgData.Count/2; i++)
            //{
            //    imgData[i].Image.Save($"Image/test_{i.ToString().PadLeft(2,'0')}.png");
            //}
            newImage.SaveAsJpeg("Image/test.jpg");
            alpha.Save("Image/clone.png");
            test.Save("Image/rand.jpg");
        }
    }

}
