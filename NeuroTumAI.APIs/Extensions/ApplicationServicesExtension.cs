using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NeuroTumAI.APIs.Errors;
using NeuroTumAI.APIs.Middlewares;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Repository;
using NeuroTumAI.Service.LocalizationService;
using System.Globalization;
using System.Text;

namespace NeuroTumAI.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{

			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<ILocalizationService, LocalizationService>();

			services.AddScoped<ExceptionMiddleware>();

			services.AddLocalization();

			services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new[]
				{
					new CultureInfo("en"),
					new CultureInfo("ar")
				};

				options.DefaultRequestCulture = new RequestCulture("en");
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;
				options.ApplyCurrentCultureToResponseHeaders = true;
			});


			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{

					var errors = actionContext.ModelState
												   .Where(P => P.Value.Errors.Count > 0)
												   .SelectMany(P => P.Value.Errors)
												   .Select(E => E.ErrorMessage)
												   .ToList();
					var response = new ApiValidationErrorResponse() { Errors = errors };

					return new BadRequestObjectResult(response);
				};
			});

			return services;
		}

		//public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
		//{

		//	services.AddIdentity<ApplicationUser, IdentityRole>()
		//			.AddEntityFrameworkStores<ApplicationIdentityDbContext>();

		//	services.AddAuthentication(options =>
		//	{
		//		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		//		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		//	}).AddJwtBearer(options =>
		//	{
		//		options.TokenValidationParameters = new TokenValidationParameters()
		//		{
		//			ValidateIssuer = true,
		//			ValidIssuer = configuration["JWT:ValidIssuer"],
		//			ValidateAudience = true,
		//			ValidAudience = configuration["JWT:ValidAudience"],
		//			ValidateIssuerSigningKey = true,
		//			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"] ?? string.Empty)),
		//			ValidateLifetime = true,
		//			ClockSkew = TimeSpan.Zero
		//		};
		//	});

		//	services.AddScoped(typeof(IAuthService), typeof(AuthService));

		//	return services;

		//}
	}
}
