using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCourse.Services
{
    public class JwtTokenFilter : IActionFilter
    {
        private readonly IMemoryCache _cache;

        public JwtTokenFilter(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            if (_cache.TryGetValue(token, out _))
            {
                context.Result = new UnauthorizedResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
