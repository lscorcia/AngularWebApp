using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AngularWebApp.Infrastructure.Web.Authentication.Repository
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }
        public ApplicationRole(string roleName): base(roleName) { }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}