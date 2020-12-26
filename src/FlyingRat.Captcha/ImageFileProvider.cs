using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Interface;
using Microsoft.Extensions.Options;
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
        private Random Random { get; } = new Random(DateTime.Now.Millisecond);
        private readonly string[] SliderPath;
        private readonly string[] LumpPath;
        public CaptchaImageOptions _imageOptions { get; set; }
        public ImageFileProvider(IOptions<CaptchaImageOptions> options)
        {
            _imageOptions = options.Value;
            SliderPath = FindDirectorys(_imageOptions.Slider_Path);
            LumpPath = FindDirectorys(_imageOptions.Lump_Path);
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
                var index = Random.Next(0, LumpPath.Length);
                return LumpPath[index];
            }
        }
        public string[] LoadFilesPath(string directory)
        {
            return FindFiles(directory);
        }
        public async Task<Image<Rgba32>> LoadBackground()
        {
            var files = FindFiles(_imageOptions.Background_Path);
            if (files == null) return default;
            var index = Random.Next(0, files.Length);
            var image = await LoadImage(files[index]);
            return image;
        }
        public Task<Image<Rgba32>> LoadAlpha(string path)
        {
            var name = Path.Combine(path, _imageOptions.Alpha_Name);
            return LoadImage(name);
        }
        public Task<Image<Rgba32>> LoadBorder(string path)
        {
            var name = Path.Combine(path, _imageOptions.Border_Name);
            return LoadImage(name);
        }
        public Task<Image<Rgba32>> LoadSlider(string path)
        {
            var name = Path.Combine(path, _imageOptions.Image_Name);
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
        public async Task<List<Image<Rgba32>>> LoadRandImages(string directory)
        {
            var images=await LoadImages(directory);
            var randImages = new List<Image<Rgba32>>(images.Count);
            var count = images.Count;
            for (int i = 0; i < count; i++)
            {
                var index=Random.Next(0, images.Count);
                randImages.Add(images[index]);
                images.RemoveAt(index);
            }
            images.Clear();
            return randImages;
        }

        private string[] FindFiles(string dir)
        {
            dir=dir.Replace('\\', Path.AltDirectorySeparatorChar);
            var str_Path =dir.StartsWith(_imageOptions.Root)?dir:Path.Combine(_imageOptions.Root, dir);
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
            var str_Path = Path.Combine(_imageOptions.Root, dir).Replace('\\',Path.AltDirectorySeparatorChar);
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
