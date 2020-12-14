using FlyingRat.Captcha.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha
{
    public class DefaultTokenGenerate : ITokenGenerate
    {
        public string Create()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
