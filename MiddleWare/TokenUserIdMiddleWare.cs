using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

namespace Ecommerce_Webapi.MiddleWare
{
    public class TokenUserIdMiddleWare
    {
        private RequestDelegate _next;
        private ILogger<TokenUserIdMiddleWare> _logger;
        public TokenUserIdMiddleWare(RequestDelegate next, ILogger<TokenUserIdMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString().Split(" ").Last();

            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwttoken = handler.ReadJwtToken(token);
                var claim = jwttoken.Claims.FirstOrDefault(cl => cl.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                foreach(var cl in jwttoken.Claims)
                {
                    Console.WriteLine($"Claim typ:{cl.Type} claim value{cl.Value}");

                }
                if (claim != null)
                {
                    var userid = claim.Value;
                    int id = Convert.ToInt32(userid);
                    context.Items["UserId"] = id;
                    _logger.LogInformation($"this is the User id : {id}");
                }
                else
                {
                   _logger.LogError("user id not found");
                }
            }
            else
            {
                _logger.LogError("Token cannot be read ");
            }
            await _next(context);
        }
    }
}
