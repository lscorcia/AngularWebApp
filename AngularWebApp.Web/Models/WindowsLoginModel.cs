using System.ComponentModel.DataAnnotations;

namespace AngularWebApp.Web.Models
{
    public class WindowsLoginModel
    {
        [Required]
        public string ClientId { get; set; }
    }
}