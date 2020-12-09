using FlyingRat.Captcha.Interface;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FlyingRat.Captcha
{
    public class ImageFileProvider : IImageProvider
    {
        private string Background_Path { get; set; } = "wwwroot/Image/Background";
        private string Slider_Path { get; set; } = "wwwroot/Image/Sliders";
        private string Lump_Path { get; set; } = "wwwroot/Image/Lump";
        private string Alpha_Name { get; set; } = "alpha.png";
        private string Border_Name { get; set; } = "border.png";
        private string Image_Name { get; set; } = "image.png";
        private Random Random { get; } = new Random(DateTime.Now.Millisecond);
        private readonly string[] SliderPath;
        public ImageFileProvider()
        {
            SliderPath = FindDirectorys(Slider_Path);
        }
        public string SliderRoot
        {
            get
            {
                var index = Random.Next(0, SliderPath.Length);
                return SliderPath[index];
            }
        }
        public string LumpRoot
        {
            get
            {
                return Lump_Path;
            }
        }
        public string[] LoadFilesPath(string directory)
        {
            return FindFiles(directory);
        }
        public async Task<Image<Rgba32>> LoadBackground()
        {
            var files = FindFiles(Background_Path);
            if (files == null) return default;
            var index = Random.Next(0, files.Length);
            var image = await LoadImage(files[index]);
            return image;
        }
        public Task<Image<Rgba32>> LoadAlpha(string path)
        {
            var name = Path.Combine(path, Alpha_Name);
            return LoadImage(name);
        }
        public Task<Image<Rgba32>> LoadBorder(string path)
        {
            var name = Path.Combine(path, Border_Name);
            return LoadImage(name);
        }
        public Task<Image<Rgba32>> LoadSlider(string path)
        {
            var name = Path.Combine(path, Image_Name);
            return LoadImage(name);
        }
        public Task<Image<Rgba32>> LoadImage(string path)
        {
            if (!File.Exists(path)) return default;
            return Image.LoadAsync<Rgba32>(path);
        }

        public async Task<List<Image<Rgba32>>> LoadImages(string directory)
        {
            var paths = FindFiles(directory) ?? new string[0];
            var images = new List<Image<Rgba32>>(paths.Length);
            for (int i = 0; i < paths.Length; i++)
            {
                var image =await LoadImage(paths[i]);
                images.Add(image);
            }
            return images;
        }

        private string[] FindFiles(string dir)
        {
            var str_Path = dir;
            //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dir);
            if (Directory.Exists(str_Path))
            {
                return Directory.GetFiles(str_Path);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到目录下的所有目录名
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private string[] FindDirectorys(string dir)
        {
            var str_Path = dir;
            //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dir);
            if (Directory.Exists(str_Path))
            {
                return Directory.GetDirectories(str_Path);
            }
            else
            {
                return null;
            }
        }

    }
}
