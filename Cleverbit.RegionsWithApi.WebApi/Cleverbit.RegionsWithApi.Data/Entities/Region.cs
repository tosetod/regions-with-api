using System.ComponentModel.DataAnnotations.Schema;

namespace Cleverbit.RegionsWithApi.Data.Entities
{
    public class Region : BaseEntity
    {
        public string Name { get; set; }
        public int? ParentRegionId { get; set; }

        [ForeignKey(nameof(ParentRegionId))]
        public virtual Region? ParentRegion { get; set; }
        public virtual ICollection<Region>? Subregions { get; set; }
        public virtual ICollection<Employee>? Employees { get; set; }
    }
}
