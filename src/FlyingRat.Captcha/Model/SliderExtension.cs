using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Model
{
    public class SliderExtension
    {
        public SliderExtension(int y,int rotate=0)
        {
            this.y = y;
            this.Rotate = rotate;
        }
        public SliderExtension() { }
        public int y { get; set; }
        public int Rotate { get; set; }
    }
}
