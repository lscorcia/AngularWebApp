using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngularWebApp.Infrastructure.ActiveDirectory.Models
{
    public class GetUserInfoByAccountNameInputDto
    {
        /// <summary>
        /// Il SamAccountName dell'utente, eventualmente prefisso con il nome del dominio (es. RETE\lucaleonardo.scorcia)
        /// In assenza di prefisso, il sistema interrogherà il dominio relativo alla macchina corrente.
        /// </summary>
        public string SamAccountName { get; set; }
    }
}
