using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngularWebApp.Infrastructure.ActiveDirectory.Models
{
    public class GetContattoUtenteByCfOutputDto
    {
        public string SamAccountName { get; set; }
        public string CodiceFiscale { get; set; }
        public List<string> Email { get; set; }
        public List<string> TelefonoServizioFisso { get; set; }
        public List<string> TelefonoServizioMobile { get; set; }

        public GetContattoUtenteByCfOutputDto()
        {
            Email = new List<string>();
            TelefonoServizioFisso = new List<string>();
            TelefonoServizioMobile = new List<string>();
        }
    }
}
