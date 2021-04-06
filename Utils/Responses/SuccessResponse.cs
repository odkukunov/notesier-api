using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notesier_API.Utils.Responses
{
    public class SuccessResponse : JSONResponse
    {
        public object Data { get; private set; }

        public SuccessResponse(object data) : base(true)
        {
            Data = data;
        }
    }
}
