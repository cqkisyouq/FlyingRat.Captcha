using FlyingRat.Captcha.Validator;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha
{
    public class RandData
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public CaptchaPoint Point { get; set; }
        public int Index { get; set; }
        public bool Change { get; set; }
        //public Image<Rgba32> Image { get; set; }
    }
}
