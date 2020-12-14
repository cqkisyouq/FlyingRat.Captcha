using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlyingRat.Captcha.Interface
{
    public interface IImageProvider
    {
        string SliderRoot { get; }
        string LumpRoot { get; }
        string[] LoadFilesPath(string directory);
        Task<Image<Rgba32>> LoadBackground();
        Task<Image<Rgba32>> LoadAlpha(string path);
        Task<Image<Rgba32>> LoadBorder(string path);
        Task<Image<Rgba32>> LoadSlider(string path);
        Task<Image<Rgba32>> LoadImage(string path);
        Task<List<Image<Rgba32>>> LoadImages(string directory);
        Task<List<Image<Rgba32>>> LoadRandImages(string directory);
    }
}
