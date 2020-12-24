using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Model;
using FlyingRat.Captcha.Validator;
using SixLabors.ImageSharp;
using System.Collections.Generic;

namespace FlyingRat.Captcha
{
    public class CaptchaCacheModel
    {
        /// <summary>
        /// Random gaps for the image
        /// </summary>
        public List<CaptchaPoint> Points { get; set; }

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
        public CaptchaType Type { get; set; }
        public string Code { get; set; }
        public string Token { get; set; }
        public ValidateResult Validate { get; set; }
        public BaseCaptchaOptions Options { get; set; }
        public string Name { get; set; }
    }
}
