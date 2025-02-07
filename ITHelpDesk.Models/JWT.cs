using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITHelpDesk.Models
{
    public class JWT
    {
        public const string Issuer = "MVS";
        public const string Audience = "ApiUser";
        public const string Key = "12345678901234567890";
        public const string AuthSchemes =
            "Identity.Application" + "," + JwtBearerDefaults.AuthenticationScheme;
    }
}
