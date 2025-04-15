using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NeuroTumAI.APIs.Errors;
using NeuroTumAI.APIs.Middlewares;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Repository;
using NeuroTumAI.Repository.Data;
using NeuroTumAI.Service.Mappings;
using NeuroTumAI.Service.Providers.Identity;
using NeuroTumAI.Service.Services.AccountService;
using NeuroTumAI.Service.Services.AuthService;
using NeuroTumAI.Service.Services.BlobStorageService;
using NeuroTumAI.Service.Services.EmailService;
using NeuroTumAI.Service.Services.LocalizationService;
using NeuroTumAI.Service.Services.PostService;
using System.Globalization;
using System.Text;

namespace NeuroTumAI.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{

			services.AddAutoMapper(typeof(MappingProfile));
			services.AddSignalR();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<ILocalizationService, LocalizationService>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IPostService, PostService>();
			services.AddScoped<IBlobStorageService, BlobStorageService>();
			services.AddTransient<IEmailService, EmailService>();
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

					return new UnprocessableEntityObjectResult(response);
				};
			});

			return services;
		}

		public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddIdentity<ApplicationUser, IdentityRole>()
					.AddEntityFrameworkStores<StoreContext>()
					.AddDefaultTokenProviders()
					.AddTokenProvider<CustomEmailTokenProvider<ApplicationUser>>("EmailConfirmation");


			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidIssuer = configuration["JWT:ValidIssuer"],
					ValidateAudience = true,
					ValidAudience = configuration["JWT:ValidAudience"],
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"] ?? string.Empty)),
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};

				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var accessToken = context.Request.Query["access_token"];

						// If the request is for the hub...
						var path = context.HttpContext.Request.Path;
						if (!string.IsNullOrEmpty(accessToken) &&
							(path.StartsWithSegments("/posthub")))
						{
							// Read the token from the query string
							context.Token = accessToken;
						}
						return Task.CompletedTask;
					}
				};
			});

			//services.AddScoped(typeof(IAuthService), typeof(AuthService));

			return services;

		}
	}
}
