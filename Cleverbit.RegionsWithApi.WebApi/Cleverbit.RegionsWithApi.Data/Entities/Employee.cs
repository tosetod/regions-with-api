using System.ComponentModel.DataAnnotations.Schema;

namespace Cleverbit.RegionsWithApi.Data.Entities
{
    public class Employee : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int RegionId { get; set; }

        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
    }
}
