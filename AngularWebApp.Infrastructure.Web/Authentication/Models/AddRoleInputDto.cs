using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AngularWebApp.Infrastructure.Web.Authentication.Models
{
    public class AddRoleInputDto
    {
        [Required]
        public string Name { get; set; }
    }
}
