namespace Clean.Web.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Net.Http.Headers;
    using System.Threading.Tasks;

    /// <summary>
    /// Adds a CacheControl header to all requests.
    /// </summary>
    public class NoCacheFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Called during action execution.
        /// </summary>
        /// <param name="context">The action context.</param>
        /// <param name="next">The next delegate</param>
        /// <returns>The response.</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var response = await next();
            if (context.HttpContext.Request.Method == "GET" && !context.HttpContext.Response.Headers.ContainsKey(HeaderNames.CacheControl))
            {
                context.HttpContext.Response.Headers[HeaderNames.CacheControl] = "private,max-age=0";
            }
        }
    }
}
