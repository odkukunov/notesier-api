using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Notesier_API.Utils.Responses
{
    public class JSONResponse
    {
        public bool Success { get; private set; }
        public JSONResponse(bool success)
        {
            Success = success;
        }
    }
}
