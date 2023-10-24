using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingPlatform.Application.Options
{
    public class TokenOption
    {
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience {  get; set; }
    }
}
