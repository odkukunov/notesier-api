using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notesier_API.Utils.Filters
{
    public class AuthAttribute : Attribute, IAsyncResourceFilter
    {
        public Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            
            string token = context.HttpContext.Request.Cookies["token"];

            if (token != null && token.StartsWith("Bearer "))
            {
                context.HttpContext.Items.Add("token", token.Remove(0, 7));
            }

            return next();
        }
    }
}
