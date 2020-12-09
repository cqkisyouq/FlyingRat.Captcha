using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha
{
    public class CaptchaViewModel
    {
        public string Index { get; set; }
        public string Change { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Col { get; set; }
        public int Row { get; set; }
        public int Y { get; set; }
        public int X { get; set; }
        public string BgGap { get; set; }
        public string Gap { get; set; }
        public string validate { get; set; }
        public string Full { get; set; }
        public string Tk { get; set; }
        public bool IsAction { get; set; }
        public CaptchaType Type { get; set; }
    }
}
