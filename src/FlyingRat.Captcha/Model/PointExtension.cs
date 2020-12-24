using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Model
{
    public class PointExtension
    {
        public PointExtension(int th, int tw)
        {
            this.th = th;
            this.tw = tw;
        }
        public PointExtension() { }
        public int th { get; set; }
        public int tw { get; set; }
    }
}
