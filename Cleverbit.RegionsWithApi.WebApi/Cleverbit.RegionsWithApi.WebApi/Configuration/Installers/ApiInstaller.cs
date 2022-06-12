using Cleverbit.RegionsWithApi.Data;
using Cleverbit.RegionsWithApi.Infrastructure.Authentication.Extensions;
using Cleverbit.RegionsWithApi.Infrastructure.Logging.Extensions;
using Cleverbit.RegionsWithApi.Infrastructure.SchemaExtensions;
using Cleverbit.RegionsWithApi.Infrastructure.Validations.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Cleverbit.RegionsWithApi.WebApi.Configuration.Installers
{
    public class ApiInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddDbContext<RegionsWithApiDbContext>((options)
                => options.UseSqlServer(configuration.GetConnectionString("RegionsWithApiDbConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllersWithViews()
                    .AddJsonOptions(options =>
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
                    );
            services.AddRazorPages();

            services.AddRequestsLoggingFilter();
            services.AddRequestsValidationFilter();
            services.AddRequestsAuthenticationFilter();

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Regions With Api API", Version = "v1" });
                options.CustomSchemaIds((t) => t.FullName.GetSchemaFileName());
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });

            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => configuration.RootPath = $"ClientApp/dist");
        }
    }
}
