using Cleverbit.RegionsWithApi.Infrastructure.ErrorHandling.Extensions;

namespace Cleverbit.RegionsWithApi.WebApi.Configuration
{
    public static class InstallerExtensions
    {
        /// <summary>
        /// Installs all services in assembly that implement IInstaller
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var installers = typeof(Program).Assembly.ExportedTypes.Where(x =>
                typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration, environment));
        }

        /// <summary>
        /// Setup application pipeline in the right order
        /// </summary>
        /// <param name="application"></param>
        public static void SetupPipeline(this WebApplication application)
        {
            if (application.Environment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
                application.UseMigrationsEndPoint();
                application.UseSwagger();
                application.UseSwaggerUI(setup =>
                {
                    setup.EnableValidator();
                });
            }
            else
            {
                application.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                application.UseHsts();
            }

            application.UseHttpsRedirection();

            application.UseSpaStaticFiles();

            application.UseRouting();

            var clientUrl = (string)application.Configuration.GetSection("AppSettings").GetValue(typeof(string), "ClientUrl");

            application.UseCors(policyBuilder =>
                policyBuilder.WithOrigins(clientUrl)
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials());

            application.UseAuthentication();
            application.UseAuthorization();

            application.MapControllers();

            application.UseErrorHandlingFilter();

            application.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";
            });
        }
    }
}
