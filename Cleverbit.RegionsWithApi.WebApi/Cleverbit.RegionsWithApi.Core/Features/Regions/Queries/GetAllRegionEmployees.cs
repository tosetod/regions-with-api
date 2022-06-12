using Cleverbit.RegionsWithApi.Common.Exceptions;
using Cleverbit.RegionsWithApi.Common.Models.DTOs.Employees;
using Cleverbit.RegionsWithApi.Common.Models.DTOs.Regions;
using Cleverbit.RegionsWithApi.Data.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cleverbit.RegionsWithApi.Core.Features.Regions.Queries
{
    public class GetAllRegionEmployees
    {
        public class Query : BaseRequest<QueryResult>
        {
            public int Id { get; set; }
        }

        public class QueryResult
        {
            public List<EmployeeDto> Employees { get; set; }
        }

        public class Validator : BaseValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class QueryHandler : BaseHandler<Query, QueryResult>
        {
            public override async Task<QueryResult> Handle(Query query, CancellationToken cancellationToken)
            {
                var regionsQuery = _ef.Regions.AsNoTracking()
                                        .Include(r => r.Subregions)
                                        .Include(r => r.Employees)
                                        .Where(c => c.Id == query.Id)
                                        .AsSplitQuery()
                                        .Select(GetRegionProjection(10, 0));

                var region = regionsQuery.FirstOrDefault();

                if (region == null)
                {
                    throw new ResourceNotFoundException(nameof(region), query.Id);
                }

                var employees = region.Employees.Union(GetAllSubregionEmployees(region)).ToList();

                return new QueryResult
                {
                    Employees = employees,
                };
            }

            private List<EmployeeDto> GetAllSubregionEmployees(RegionDto region)
            {
                var employees = new List<EmployeeDto>();

                foreach (var subregion in region.Subregions)
                {
                    employees.AddRange(subregion.Employees);

                    foreach (var subsubregion in subregion.Subregions)
                    {
                        employees.AddRange(subsubregion.Employees);
                    }
                }

                return employees;
            }

            private static Expression<Func<Region, RegionDto>> GetRegionProjection(int maxDepth, int currentDepth = 0)
            {
                currentDepth++;

                Expression<Func<Region, RegionDto>> result = region => new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Subregions = currentDepth == maxDepth
                        ? new List<RegionDto>()
                        : region.Subregions.AsQueryable()
                                  .Select(GetRegionProjection(maxDepth, currentDepth))
                                  .ToList(),
                    Employees = region.Employees.Select(e => MapEmployeeToEmployeeDto(e, region)).ToList()
                };

                return result;
            }

            private static RegionDto MapRegionToRegionDto(Region region)
            {
                return new RegionDto
                {
                    Id = region.Id,
                    Name = region.Name
                };
            }

            private static EmployeeDto MapEmployeeToEmployeeDto(Employee employee, Region region)
            {
                return new EmployeeDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Surname = employee.Surname,
                    Region = MapRegionToRegionDto(region)
                };
            }
        }
    }
}
