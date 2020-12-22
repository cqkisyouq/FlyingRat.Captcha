using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Configuration
{
    public class CaptchaImageOptions
    {
        public string Root { get; set; } = "wwwroot/js/flyingrat";
        public string Background_Path { get; set; } = "Image/Background";
        public string Slider_Path { get; set; } = "Image/Sliders";
        public string Lump_Path { get; set; } = "Image/Lump";
        public string Alpha_Name { get; set; }= "alpha.png";
        public string Border_Name { get; set; } = "border.png";
        public string Image_Name { get; set; } = "image.png";
    }
}
