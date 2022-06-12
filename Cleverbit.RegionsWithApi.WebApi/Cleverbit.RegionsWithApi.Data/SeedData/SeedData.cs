using Cleverbit.RegionsWithApi.Data.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Cleverbit.RegionsWithApi.Data.SeedData
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<RegionsWithApiDbContext>();

            using (var transaction = context.Database.BeginTransaction())
            {

                context.Database.Migrate();
                context.Database.EnsureCreated();

                using (var reader = new StreamReader(@"regions.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<RegionMap>();
                    var regions = csv.GetRecords<Region>();

                    context.Regions.AddRange(regions);
                }

                using (var reader = new StreamReader(@"employees.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<EmployeeMap>();
                    var employess = csv.GetRecords<Employee>();

                    context.Employees.AddRange(employess);
                }

                //context.Database.ExecuteSqlRaw(_seedQuery);

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Regions ON;");

                context.SaveChanges();

                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Regions OFF;");

                transaction.Commit();
            }
        }

        #region SeedQuery
        private static string _seedQuery = "INSERT INTO ()";
        #endregion SeedQuery

        public sealed class RegionMap : ClassMap<Region>
        {
            public RegionMap()
            {
                Map(m => m.Name).Index(0);
                Map(m => m.Id).Index(1);
                Map(m => m.ParentRegionId).Index(2);
            }
        }

        public sealed class EmployeeMap : ClassMap<Employee>
        {
            public EmployeeMap()
            {
                Map(m => m.RegionId).Index(0);
                Map(m => m.Name).Index(1);
                Map(m => m.Surname).Index(2);
            }
        }
    }
}
