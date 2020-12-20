using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Validator
{
    public class CaptchaPoint
    {
        public CaptchaPoint() { }
        public CaptchaPoint(int x,int y,int width=0,int height=0)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Point Point { get { return new Point(X, Y); } }
    }
}
