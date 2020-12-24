using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Configuration
{
    public class CaptchaImageOptions
    {
        public string Root { get; set; } = "wwwroot/flyingrat";
        public string Background_Path { get; set; } = "image/background";
        public string Slider_Path { get; set; } = "Image/sliders";
        public string Lump_Path { get; set; } = "Image/lump";
        public string Alpha_Name { get; set; }= "alpha.png";
        public string Border_Name { get; set; } = "border.png";
        public string Image_Name { get; set; } = "image.png";
    }
}
