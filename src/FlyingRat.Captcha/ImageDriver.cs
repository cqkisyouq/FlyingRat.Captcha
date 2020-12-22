using FlyingRat.Captcha.Validator;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
namespace FlyingRat.Captcha
{
    public class ImageDriver
    {
        Random rd = new Random(DateTime.Now.Millisecond);
        public List<RandData> RandImagesBy(Image source, ref int col,ref int row)
        {
            if (source == null || source.Width ==0 || source.Height == 0) return default;
            col = Math.Max(1, col);
            row = Math.Max(1, row);
            var width = Math.Max((int)Math.Floor(source.Width * 1.0 / col),1);
            var height =Math.Max((int)Math.Floor(source.Height * 1.0 / row),1);
            col = source.Width / width;
            row = source.Height / height;
            List<RandData> randData = GenerateData(col, row);
            List<RandData> images = new List<RandData>(randData.Count);
            Stack<RandData> lastData = new Stack<RandData>(row);
            for (int i = 0,rcount=randData.Count; i < rcount; i++)
            {
                var imgData = Rand(randData);
                imgData.Point = new CaptchaPoint(imgData.Col * width, imgData.Row * height,width,height);
                var curWidth = width;
                var curHeight = height;
                images.Add(imgData);
                if (imgData.Col == col - 1)
                {
                    curWidth = Math.Max(source.Width - imgData.Point.X, 0);
                    curWidth = Math.Max(curWidth,0);
                    imgData.Change = true;
                    images.Remove(imgData);
                    if (curWidth == 0) continue;
                    lastData.Push(imgData);
                }
                //imgData.Image = new Image<Rgba32>(curWidth, curHeight);
                //Copy(source, imgData.Point.Point(), imgData.Image);
                imgData.Point.Width = curWidth;
                imgData.Point.Height = curHeight;
            }

            var index = 0;
            while (lastData.TryPop(out RandData image))
            {
                if (images.Count == 0)
                {
                    images.Add(image);
                }
                else
                {
                    var reIndex = rd.Next(index * col, index * col + col);
                    images.Insert(Math.Min(reIndex, images.Count - 1), image);
                }
                index++;
            }
            return images;
        }

        public Image ImageByRandImages(Image source,List<RandData> images,int col=1)
        {
            if (!images?.Any() ?? true) return source;
            col = Math.Max(1, col);
            using var cloneSource = source.CloneAs<Rgba32>();
            var newImage = source.CloneAs<Rgba32>();
            newImage.Mutate(x =>
            {
                var xpos = 0;
                for (int i = 0; i < images.Count; i++)
                {
                    var imgData = images[i];
                    using var img =new Image<Rgba32>(imgData.Point.Width, imgData.Point.Height);
                    Copy(cloneSource, imgData.Point.Point, img);
                    var xx = i % col;
                    var xy = i / col;
                    if (xx == 0) xpos = 0;
                    x.DrawImage(img, new Point(xpos, xy * img.Height), 1f);
                    //x.DrawImage(img.Image, img.Point, 1f);
                    xpos += img.Width;
                }
            });
            return newImage;
        }

        public void CopyNoAlpha(Image<Rgba32> source, Point position, Image<Rgba32> target) => Copy(source, position, target, false);
        public void Copy(Image<Rgba32> source, Point position, Image<Rgba32> target) => Copy(source, position, target,true);
        private void Copy(Image<Rgba32> source, Point position, Image<Rgba32> target,bool alpha)
        {
            var curWidth = Math.Max(0, source.Width - position.X);
            var width = Math.Min(target.Width, curWidth);
            if (width == 0 || position.Y >= source.Height) return;
            var height = Math.Min(position.Y + target.Height, source.Height);
            for (int i = position.Y, y = 0; i < height; i++, y++)
            {
                var row = source.GetPixelRowSpan(i).Slice(position.X, width);
                var targetRow = target.GetPixelRowSpan(y);
                if (alpha)
                {
                    row.CopyTo(targetRow);
                    continue;
                }
                for (int x = 0; x < row.Length; x++)
                {
                    targetRow[x].Rgb = row[x].Rgb;
                }
            }
        }
        private List<RandData> GenerateData(int col, int row)
        {
            List<RandData> images = new List<RandData>(col*row);
            var index = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    images.Add(new RandData() { Col = j, Row = i,Index=index++});
                }
            }
            return images;
        }
        private RandData Rand(List<RandData> items)
        {
            var index = rd.Next(0, items.Count);
            var data = items[index];
            items.Remove(data);
            return data;
        }
    }
}
