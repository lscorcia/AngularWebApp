using System.Collections.Generic;

namespace AngularWebApp.Infrastructure.ActiveDirectory.Models
{
    public class GetLdapUserInfoByAccountNameOutputDto
    {
        public string SamAccountName { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string CodiceFiscale { get; set; }
        public string Direzione { get; set; }
        public string Divisione { get; set; }
        public string Manager { get; set; }
        public string ManagerSamAccountName { get; set; }
        public List<string> Email { get; set; }
        public List<string> TelefonoServizioFisso { get; set; }
        public List<string> TelefonoServizioMobile { get; set; }

        public GetLdapUserInfoByAccountNameOutputDto()
        {
            Email = new List<string>();
            TelefonoServizioFisso = new List<string>();
            TelefonoServizioMobile = new List<string>();
        }
    }
}