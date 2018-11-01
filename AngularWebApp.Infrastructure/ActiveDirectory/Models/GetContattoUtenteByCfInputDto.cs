using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngularWebApp.Infrastructure.ActiveDirectory.Models
{
    public class GetContattoUtenteByCfInputDto
    {
        public string CodiceFiscale { get; set; }
        public string LdapUsersFilter { get; set; }
    }
}
