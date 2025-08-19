using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace server.Middlewares{

    public class JwtMiddleware{
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;
        private readonly string _secret;

        public JwtMiddleware(IConfiguration configuration, RequestDelegate next){
            _next = next;
            _configuration = configuration;
            _secret = configuration["Jwt:Secret"] 
                ?? throw new ArgumentNullException("Jwt:Secret is missing in configuration");
        }

         public async Task Invoke(HttpContext context)
        {
            try
            {
                //get token from Authorization header
                var token = context.Request.Headers["Authorization"]
                    .FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                    AttachUserToContext(context, token);
            }
            catch
            {
                //token validation failed, do nothing (user remains unauthenticated)
            }

            //pass request to next middleware or controller
            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);


            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, //set true to validate issuer
                ValidateAudience = false, //set true to validate audience
                ClockSkew = System.TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;

            //attach user info to context for controllers to access
            context.Items["UserId"] = userId;
        }

    }
}