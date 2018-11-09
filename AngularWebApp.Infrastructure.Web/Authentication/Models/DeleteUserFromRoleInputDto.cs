using System.ComponentModel.DataAnnotations;

namespace AngularWebApp.Infrastructure.Web.Authentication.Models
{
    public class DeleteUserFromRoleInputDto
    {
        [Required]
        public string RoleName { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}