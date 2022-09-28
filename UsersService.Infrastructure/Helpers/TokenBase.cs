using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Infrastructure.Helpers
{
    public class TokenBase
    {
        public string acces_token { get; set; }

        public string getcustomerId()
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(acces_token);
            return jwtSecurityToken.Claims.First(claim => claim.Type == "cid").Value;
        }

        public string getobjectid()
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(acces_token);
            return jwtSecurityToken.Claims.First(claim => claim.Type == "oid").Value;
        }
        public string getrole()
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(acces_token);
            return jwtSecurityToken.Claims.First(claim => claim.Type == "roles").Value;
        }
    }
}
