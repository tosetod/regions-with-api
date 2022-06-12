using Microsoft.AspNetCore.Identity;

namespace Cleverbit.RegionsWithApi.Data.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
