namespace Clean.Web.Application.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// System wide model state validation.
    /// </summary>
    public class ModelStateValidationFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Called when the action is being executed.
        /// </summary>
        /// <param name="context">The current request context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
