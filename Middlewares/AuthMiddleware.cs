using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Notesier_API.Models;
using Notesier_API.Utils.Services;
using Notesier_API.Utils.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Notesier_API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private UserModelService userModelService;
        private JWTHandler jWTHandler;

        public AuthMiddleware(RequestDelegate next, JWTHandler _jWTHandler, UserModelService _userModelService)
        {
            jWTHandler = _jWTHandler;
            userModelService = _userModelService;
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            string token = httpContext.Request.Cookies["token"];

            if (token != null && token.StartsWith("Bearer "))   
            {
                try
                {
                    ClaimsPrincipal claims = jWTHandler.Validate(token.Remove(0, 7));
                    UserModel user = userModelService.GetUserByName(claims.Claims.First().Value);

                    httpContext.Items.Add("User", user);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
           

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
