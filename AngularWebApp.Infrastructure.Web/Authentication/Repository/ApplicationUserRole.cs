using Microsoft.AspNetCore.Identity;

namespace AngularWebApp.Infrastructure.Web.Authentication.Repository
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}