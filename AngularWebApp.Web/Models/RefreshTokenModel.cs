using System.ComponentModel.DataAnnotations;

namespace AngularWebApp.Web.Models
{
    public class RefreshTokenModel
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}