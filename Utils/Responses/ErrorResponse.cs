using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notesier_API.Utils.Responses
{
    public class ErrorResponse : JSONResponse
    {
        public object Error { get; private set; }

        public ErrorResponse(object error) : base(false)
        {
            Error = error;
        }
    }
}
