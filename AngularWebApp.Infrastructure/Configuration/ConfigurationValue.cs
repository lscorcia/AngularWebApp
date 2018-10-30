using System.ComponentModel.DataAnnotations;

namespace AngularWebApp.Infrastructure.Configuration
{
    public class ConfigurationValue
    {
        [Key]
        [MaxLength(255)]
        public string Area { get; set; }

        [Key]
        [MaxLength(255)]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
        public string Description { get; set; }
    }
}