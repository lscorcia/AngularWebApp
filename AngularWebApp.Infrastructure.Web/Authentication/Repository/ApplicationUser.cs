using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AngularWebApp.Infrastructure.Web.Authentication.Repository
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}