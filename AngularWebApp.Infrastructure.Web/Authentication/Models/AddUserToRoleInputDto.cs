using System.ComponentModel.DataAnnotations;

namespace AngularWebApp.Infrastructure.Web.Authentication.Models
{
    public class AddUserToRoleInputDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Role { get; set; }
    }
}