using System.Collections.Generic;

namespace FlyingRat.Captcha.Validator
{
    public class ValidateModel
    {
        public ValidateModel() { }
        public ValidateModel(CaptchaPoint point)
        {
            if (Points == null) Points = new List<CaptchaPoint>();
            Points.Add(point);
        }
        public ValidateModel(List<CaptchaPoint> points)
        {
            Points = points;
        }
        public ValidateModel(string code)
        {
            this.Code = code;
        }
        public List<CaptchaPoint> Points { get; set; }
        public string Code { get; set; }
    }
}
