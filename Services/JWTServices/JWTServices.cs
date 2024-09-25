using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce_Webapi.Services.JWTServices
{
    public class JWTServices : IJWTServices
    {
        private IConfiguration configuration;
        private string securitykey;
        public JWTServices(IConfiguration configuration) 
        {
            this.configuration = configuration;
            securitykey = configuration["Jwt:Key"];

        }
        public int GetUserId(string token)
        {

            var Tokenhandler = new JwtSecurityTokenHandler();
            var validation = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitykey))
            };
            var res = Tokenhandler.ValidateToken(token, validation, out var validatedtoken);
            if(validatedtoken is not JwtSecurityToken jwttoken)
            {
                throw new Exception("this token is invalid");
            }
            var userclaim = jwttoken.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier);
            if (userclaim == null) 
            {
                throw new Exception("invalid or missing token id");

            }
            return Convert.ToInt32(userclaim.Value);

        }
    }
}
