using Cleverbit.RegionsWithApi.Data.Entities;
using Cleverbit.RegionsWithApi.Data.Identity;
using Cleverbit.RegionsWithApi.Data.ModelConfigurations;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Cleverbit.RegionsWithApi.Data
{
    public class RegionsWithApiDbContext : KeyApiAuthorizationDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public RegionsWithApiDbContext(DbContextOptions<RegionsWithApiDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
           : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            NNRelationshipConfiguration.Apply(modelBuilder);
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }

    public class RegionsWithApiDbContextFactory : IDesignTimeDbContextFactory<RegionsWithApiDbContext>
    {
        // IDesignTimeDbContextFactory is used usually when you execute EF Core commands like Add-Migration, Update-Database, and so on
        public RegionsWithApiDbContext CreateDbContext(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{envName}.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("RegionsWithApiDbConnection");

            var optionsBuilder = new DbContextOptionsBuilder<RegionsWithApiDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new RegionsWithApiDbContext(optionsBuilder.Options, new OperationalStoreOptionsMigrations());
        }
    }

    public class OperationalStoreOptionsMigrations : IOptions<OperationalStoreOptions>
    {
        public OperationalStoreOptions Value => new()
        {
            DeviceFlowCodes = new TableConfiguration("DeviceCodes"),
            EnableTokenCleanup = false,
            PersistedGrants = new TableConfiguration("PersistedGrants"),
            TokenCleanupBatchSize = 100,
            TokenCleanupInterval = 3600,
        };
    }
}
