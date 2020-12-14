using FlyingRat.Captcha.Validator;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha
{
    public class CaptchaImage
    {
        public string Token { get; set; }
        public string Name { get; set; }
        public CaptchaType Type { get; set; }
        /// <summary>
        /// Splite column for the image with x
        /// </summary>
        public int Col { get; set; }
        /// <summary>
        /// Splite row for the image with y
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        /// Image Indexs
        /// </summary>
        public int[] Index { get; set; }
        /// <summary>
        /// Indexs change width or height for the image
        /// </summary>
        public int[] Change { get; set; }

        /// <summary>
        /// Random gaps for the image
        /// </summary>
        public List<CaptchaPoint> Points { get; set; }

        /// <summary>
        /// Random for the image
        /// </summary>
        public List<RandData> RandData { get; set; }
        /// <summary>
        /// Full image
        /// </summary>
        public Image Backgorund { get; set; }
        /// <summary>
        /// Image after random gaps
        /// </summary>
        public Image GapBackground { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Image Gap { get; set; }
        public string Code { get; set; }
        public string Tips { get; set; }
        public int TipWidth { get; set; }
        public int tipHeight { get; set; }
    }

    public enum CaptchaType
    {
        Unknown,
        Code,
        Slider,
        Point,
        /// <summary>
        /// User defined 1
        /// </summary>
        Custom,
        /// <summary>
        /// User defined 2
        /// </summary>
        Normal
    }
}
