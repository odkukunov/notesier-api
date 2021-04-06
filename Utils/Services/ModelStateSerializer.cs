using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notesier_API.Utils.Services
{
    public static class ModelStateSerializerExtension
    {
        public static void AddModelStateSerializer(this IServiceCollection services)
        {
            services.AddSingleton<ModelStateSerializer>();
        }
    }

    public class ModelStateSerializer
    {
        public List<string> Serialize(ModelStateDictionary state)
        {
            List<string> errors = new List<string>();

            foreach(ModelStateEntry value in state.Values)
            {
                errors.AddRange(from _errors in value.Errors select _errors.ErrorMessage);
            }

            return errors;
        }
    }
}
