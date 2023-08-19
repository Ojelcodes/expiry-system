using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public static class TokenLifetimeValidator
    {
        public static bool Validate(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken tokenToValidate,
            TokenValidationParameters @param
        )
        {
            return (expires != null && expires > DateTime.UtcNow);
        }
    }
}
