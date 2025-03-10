using NeuroTumAI.APIs.Swagger;

namespace NeuroTumAI.APIs.Extensions
{
	public static class SwaggerServicesExtension
	{
		public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
		{

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(options =>
			{
				// Add Accept-Language header globally
				options.OperationFilter<LanguageHeaderOperationFilter>();
			});

			return services;

		}

		public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
		{

			app.UseSwagger();
			app.UseSwaggerUI();

			return app;
		}
	}
}
