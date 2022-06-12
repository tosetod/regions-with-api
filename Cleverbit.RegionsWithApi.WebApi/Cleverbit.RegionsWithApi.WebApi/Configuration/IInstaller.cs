namespace Cleverbit.RegionsWithApi.WebApi.Configuration
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment);
    }
}
