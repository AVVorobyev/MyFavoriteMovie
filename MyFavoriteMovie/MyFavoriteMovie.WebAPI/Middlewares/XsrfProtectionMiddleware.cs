using Microsoft.AspNetCore.Antiforgery;

namespace MyFavoriteMovie.WebAPI.Middlewares
{
    public class XsrfProtectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiforgery;

        public XsrfProtectionMiddleware(RequestDelegate requestDelegate, IAntiforgery antiforgery)
        {
            _next = requestDelegate;
            _antiforgery = antiforgery;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Cookies.Append(
                ".AspNetCore.Xsrf",
                _antiforgery.GetAndStoreTokens(context).RequestToken!,
                new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                }
            );

            await _next(context);
        }
    }
}
