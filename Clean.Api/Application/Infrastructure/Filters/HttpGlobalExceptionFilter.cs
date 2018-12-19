namespace Clean.Web.Application.Infrastructure
{
    using Clean.Web.Application.Infrastructure.ActionResults;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Net;

    /// <summary>
    /// Implements a global exception filter to make sure we are returning the right response.
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment env;
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGlobalExceptionFilter"/> class.
        /// </summary>
        /// <param name="env">The current environment.</param>
        /// <param name="logger">The current logger.</param>
        public HttpGlobalExceptionFilter(IHostingEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        /// <summary>
        /// Executes when an exception is thrown.
        /// </summary>
        /// <param name="context">The context the exception occurred.</param>
        public void OnException(ExceptionContext context)
        {
            JsonErrorResponse json;

            switch (context.Exception)
            {
                case OperationAbortedException operationAbortedException:
                    json = HandleGenericExceptionWithStatusCode(context, operationAbortedException, (int)HttpStatusCode.Conflict);
                    break;
                case KeyNotFoundException keyNotFoundException:
                    json = HandleKeyNotFoundException(context, keyNotFoundException);
                    break;
                case NotImplementedException notImplementedException:
                    json = HandleGenericExceptionWithStatusCode(context, notImplementedException, (int)HttpStatusCode.NotImplemented);
                    break;
                default:
                    json = HandleGenericException(context);
                    break;
            }
        }

        private JsonErrorResponse HandleGenericException(ExceptionContext context)
        {
            logger.LogError(
                new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            JsonErrorResponse json = new JsonErrorResponse
            {
                Messages = new[] { "An error has occurred.  If this error continues, please contact Customer Support." }
            };
            if (env.IsDevelopment())
            {
                json.DeveloperMessage = context.Exception;
            }

            context.Result = new InternalServerErrorObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return json;
        }

        private JsonErrorResponse HandleKeyNotFoundException(ExceptionContext context, KeyNotFoundException keyNotFoundException)
        {
            JsonErrorResponse json = new JsonErrorResponse
            {
                Messages = new[] { "Item not found." }
            };

            if (env.IsDevelopment())
            {
                json.DeveloperMessage = context.Exception;
            }

            // Result asigned to a result object but in destiny the response is empty. This is a known bug of .net core 1.1
            // It will be fixed in .net core 1.1.2. See https://github.com/aspnet/Mvc/issues/5594 for more information
            context.Result = new NotFoundObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return json;
        }

        private JsonErrorResponse HandleGenericExceptionWithStatusCode(ExceptionContext context, Exception exception, int statusCode)
        {
            JsonErrorResponse json = new JsonErrorResponse
            {
                Messages = new[] { exception.Message }
            };

            // if (env.IsDevelopment())
            // {
            //    json.DeveloperMessage = context.Exception;
            // }

            // Result asigned to a result object but in destiny the response is empty. This is a known bug of .net core 1.1
            // It will be fixed in .net core 1.1.2. See https://github.com/aspnet/Mvc/issues/5594 for more information
            context.Result = new ObjectResult(json);
            context.HttpContext.Response.StatusCode = statusCode;
            return json;
        }

        private class JsonErrorResponse
        {
            public string[] Messages { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object DeveloperMessage { get; set; }
        }
    }
}
