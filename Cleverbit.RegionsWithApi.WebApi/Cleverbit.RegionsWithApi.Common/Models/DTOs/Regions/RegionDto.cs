using Cleverbit.RegionsWithApi.Common.Models.DTOs.Employees;

namespace Cleverbit.RegionsWithApi.Common.Models.DTOs.Regions
{
    public class RegionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RegionDto> Subregions { get; set; }
        public List<EmployeeDto> Employees { get; set; }
    }
}
