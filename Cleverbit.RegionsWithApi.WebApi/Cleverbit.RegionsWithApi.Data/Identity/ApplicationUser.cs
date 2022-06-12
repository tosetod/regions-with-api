using Microsoft.AspNetCore.Identity;

namespace Cleverbit.RegionsWithApi.Data.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public Guid CreatedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;
        public Guid ModifiedBy { get; set; }
    }
}
