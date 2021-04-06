using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notesier_API.Utils
{
    public class AuthOptions
    {
        public const string ISSUER = "Notesier-Server";
        public const string AUDIENCE = "Notesier-Client";
        const string KEY = "2281337rubtid1488";
        public const int LIFETIME = 30;
        public static SymmetricSecurityKey GetSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
