using FlyingRat.Captcha.Configuration;
using FlyingRat.Captcha.Interface;
using FlyingRat.Captcha.Model;
using FlyingRat.Captcha.Validator;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlyingRat.Captcha
{
    public abstract class BaseCaptcha : ICaptcha
    {
        protected Random rd = new Random(DateTime.Now.Millisecond);

        public string Name => this.GetType().Name;

        public abstract CaptchaType Type { get; }

        public ValueTask<CaptchaImage> Captcha(BaseCaptchaOptions options = null)
        {
            return CaptchaCreate(options);
        }

        public abstract ValueTask<CaptchaImage> CaptchaCreate(BaseCaptchaOptions options = null);
        protected List<CaptchaPoint> RandPoints(Image source, params Image[] target)
        {
            var points = new List<CaptchaPoint>();
            if (target == null || target.Length==0) return points;
            for (int i = 0; i < target.Length; i++)
            {
                var point = RandPoint(source, target[i],points);
                points.Add(point);
            }
            return points;
        }

        protected CaptchaPoint RandPoint(Image source, Image target)
        {
            var x = rd.Next(target.Width, source.Width - (int)(target.Width * 1.5));
            var y = rd.Next(target.Height, source.Height - (int)(target.Height * 1.5));
            return new CaptchaPoint(x, y, target.Width, target.Height);
        }

        protected CaptchaPoint RandPoint(Image source, Image target, List<CaptchaPoint> points)
        {
            var maxWidth = source.Width - (int)(target.Width * 1.5);
            var maxHeight = source.Height - (int)(target.Height * 1.5);
            var x = rd.Next(target.Width, maxWidth);
            var y = rd.Next(target.Height, maxHeight);
            var point = new CaptchaPoint(x, y, target.Width, target.Height);
            if (points.Count <= 0) return point;
            while (hasPoint(point, points))
            {
                point.X = rd.Next(target.Width, maxWidth);
                point.Y= rd.Next(target.Height, maxHeight);
            }
            return point;
        }

        protected CaptchaPoint RandPoint(List<CaptchaPoint> points)
        {
            if (points == null || points.Count == 0) return default;
            var index = rd.Next(0, points.Count);
            var point=points[index];
            if(point!=null) points.Remove(point);
            return point;
        }
        protected bool hasPoint(CaptchaPoint point,List<CaptchaPoint> points)
        {
            if (points.Count <= 0) return false;
            for (int i = 0; i < points.Count; i++)
            {
                var oldp = points[i];
                var bmw = Math.Max(point.Width, oldp.Width);
                var bmh = Math.Max(point.Height, oldp.Height);
                if (Math.Abs(point.X - oldp.X)<=bmw
                    && Math.Abs(point.Y-oldp.Y)<=bmh)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
