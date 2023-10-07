using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GeekBurger.Products.Helper
{
    public class UnprocessableEntityResult : ObjectResult
    {
        public UnprocessableEntityResult(ModelStateDictionary modelState) : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }
            StatusCode = 422;
        }
    }
}
