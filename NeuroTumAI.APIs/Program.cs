using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NeuroTumAI.APIs.Extensions;
using NeuroTumAI.APIs.Middlewares;
using NeuroTumAI.Core.Resources.Validation;
using NeuroTumAI.Repository.Data;
using Newtonsoft.Json;

namespace NeuroTumAI.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			#region Configure Services

			builder.Services.AddControllers().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			}).AddDataAnnotationsLocalization(options =>
			{
				options.DataAnnotationLocalizerProvider = (type, factory) =>
					factory.Create(typeof(ValidationResources));
			});
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

			builder.Services.AddSwaggerServices();

			builder.Services.AddApplicationServices();

			builder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddAuthServices(builder.Configuration);

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("MyPolicy", policyOptions =>
				{
					policyOptions.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
				});
			});

			#endregion

			var app = builder.Build();

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;


			var _dbContext = services.GetRequiredService<StoreContext>();

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				await _dbContext.Database.MigrateAsync();

				var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

				await StoreContextSeed.SeedAsync(_dbContext, roleManager);

			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "an error has been occured during apply the migration");
			}

			#region Configure Kestrel Middlwares

			// Get localization options from DI
			var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
			app.UseRequestLocalization(localizationOptions);

			app.UseMiddleware<ExceptionMiddleware>();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddlewares();
			}

			app.UseStatusCodePagesWithReExecute("/Errors/{0}");

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseCors("MyPolicy");

			app.MapControllers();

			#endregion

			app.Run();
		}
	}
}
