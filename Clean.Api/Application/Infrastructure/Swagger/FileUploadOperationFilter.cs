namespace CleanArchictecture.Web.Infrastructure.Swagger
{
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// This operation will provide the option to modify or replace the operation parameters for a file upload.
    /// </summary>
    public class FileUploadOperationFilter : IOperationFilter
    {
        /// <summary>
        /// This applies the filter
        /// </summary>
        /// <param name="operation">The operation to be filtered</param>
        /// <param name="context">The current operation filter context</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var parameters = context.ApiDescription.ParameterDescriptions.Where(x => x.ModelMetadata?.ModelType == typeof(IFormFile));

            foreach (var parameter in parameters)
            {
                var param = operation.Parameters.Where(p => p.Name == parameter.Name).FirstOrDefault();
                if (param != null)
                {
                    operation.Parameters.Remove(param);
                    operation.Parameters.Add(new NonBodyParameter
                    {
                        Name = param.Name,
                        In = "formData",
                        Description = param.Description,
                        Required = param.Required,
                        Type = "file"
                    });
                    if (!operation.Consumes.Contains("application/form-data"))
                    {
                        operation.Consumes.Add("application/form-data");
                    }
                }
            }
        }
    }
}
