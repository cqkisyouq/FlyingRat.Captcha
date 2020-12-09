using FlyingRat.Captcha.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlyingRat.Captcha.Context
{
    public class CaptchaContext
    {
        public CaptchaContext(ValidateModel source,ValidateModel validate,ValidateResult result)
        {
            Source = source;
            Validate = validate;
            Result = result;
        }
        public CaptchaContext(ValidateModel source, ValidateModel validate) : this(source, validate, null)
        {

        }
        private ValidateResult Result { get; set; }
        public ValidateModel Source { get; set; }
        public ValidateModel Validate { get; set; }
        public CaptchaType Type { get; set; }

        public ValidateResult GetResult()
        {
            if (Result == null) Result = ValidateResult.Failed;
            return this.Result;
        }
    }
}
