using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeuroTumAI.APIs.Swagger
{
	public class LanguageHeaderOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			operation.Parameters.Add(new OpenApiParameter
			{
				Name = "Accept-Language",
				In = ParameterLocation.Header,
				Required = false,
				Schema = new OpenApiSchema { Type = "string", Default = new Microsoft.OpenApi.Any.OpenApiString("en") }
			});
		}
	}
}
