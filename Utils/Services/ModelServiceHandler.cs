using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notesier_API.Utils.Services.ModelServices;

namespace Notesier_API.Utils.Services
{

    public static class ModelServiceHandlerExtension 
    {
        public static void AddModelServiceHandling(this IServiceCollection services)
        {
            services.AddScoped<UserModelService>();
        }
    }
}
