using Microsoft.Extensions.Options;
using Token.API.Helpers;
using Token.API.Services;

namespace Token.API.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach user to context on successfull jwt validation
                context.Items["User"] = userService.GetById(userId.Value);
            }

            await _next(context);
        }
        /*Token authentication başarısız olursa veya token yoksa, 
         * HTTP bağlamına eklenmiş kimliği doğrulanmış bir kullanıcı nesnesi olmadığından requestin yalnızca anonim erişmesine izin verilir. */
    }
}
