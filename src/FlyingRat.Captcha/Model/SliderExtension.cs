using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Model
{
    public class SliderExtension
    {
        public SliderExtension(int y)
        {
            this.y = y;
        }
        public SliderExtension() { }
        public int y { get; set; }
    }
}
