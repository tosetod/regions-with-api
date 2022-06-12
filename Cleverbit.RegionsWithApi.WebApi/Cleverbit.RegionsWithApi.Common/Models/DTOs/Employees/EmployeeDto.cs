using Cleverbit.RegionsWithApi.Common.Models.DTOs.Regions;

namespace Cleverbit.RegionsWithApi.Common.Models.DTOs.Employees
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public RegionDto Region { get; set; }
    }
}
