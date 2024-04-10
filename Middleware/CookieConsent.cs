namespace IntexBrickwell.Middleware
{
    public class CookieConsent
    {
        private readonly RequestDelegate _next;

        public CookieConsent(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // Your cookie consent checking logic here
            await _next(httpContext);
        }
    }
}
