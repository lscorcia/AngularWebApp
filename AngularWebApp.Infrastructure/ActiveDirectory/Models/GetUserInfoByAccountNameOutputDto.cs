using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngularWebApp.Infrastructure.ActiveDirectory.Models
{
    public class GetUserInfoByAccountNameOutputDto
    {
        public string DisplayName { get; set; }
        public string SamAccountName { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
