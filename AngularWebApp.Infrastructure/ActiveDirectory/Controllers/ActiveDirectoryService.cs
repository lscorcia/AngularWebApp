using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using Microsoft.Extensions.Configuration;
using AngularWebApp.Infrastructure.ActiveDirectory.Models;
using AngularWebApp.Infrastructure.DI;

namespace AngularWebApp.Infrastructure.ActiveDirectory.Controllers
{
    public class ActiveDirectoryService: IApplicationService
    {
        public IConfiguration Configuration { get; set; }
        protected string ActiveDirectoryLdapUrl { get { return Configuration.GetValue<string>("MiSE.ActiveDirectory.LdapUrl"); } }

        public ActiveDirectoryService(IConfiguration _config)
        {
            this.Configuration = _config;
        }

        internal List<GetContattoUtenteByCfOutputDto> GetContattoUtenteByCf(List<GetContattoUtenteByCfInputDto> lstDto)
        {
            List<GetContattoUtenteByCfOutputDto> output = new List<GetContattoUtenteByCfOutputDto>();

            foreach (var item in lstDto)
            {
                var dtoContattoUtente = GetContattoUtenteByCf(item);
                output.AddRange(dtoContattoUtente);
            }

            return output;
        }

        private List<GetContattoUtenteByCfOutputDto> GetContattoUtenteByCf(GetContattoUtenteByCfInputDto dto)
        {
            if (String.IsNullOrEmpty(dto.CodiceFiscale))
                throw new ArgumentNullException(nameof(dto.CodiceFiscale));

            List<GetContattoUtenteByCfOutputDto> output = new List<GetContattoUtenteByCfOutputDto>();

            DirectoryEntry searchRoot = new DirectoryEntry(ActiveDirectoryLdapUrl);

            using (DirectorySearcher search = new DirectorySearcher(searchRoot))
            {
                if (!String.IsNullOrEmpty(dto.LdapUsersFilter))
                    search.Filter = String.Format("(&" + dto.LdapUsersFilter + "(extensionAttribute1={0}))", dto.CodiceFiscale);
                else
                    search.Filter = String.Format("(extensionAttribute1={0})", dto.CodiceFiscale);

                search.PropertiesToLoad.Add("SAMAccountName");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("extensionAttribute1");
                search.PropertiesToLoad.Add("telephoneNumber");
                search.PropertiesToLoad.Add("mobile");
                search.PageSize = 1000;

                var resultCol = SafeFindAll(search);

                if (resultCol != null)
                {
                    foreach (var result in resultCol)
                    {
                        GetContattoUtenteByCfOutputDto item = new GetContattoUtenteByCfOutputDto();

                        if (result.Properties["SAMAccountName"].Count > 0)
                            item.SamAccountName = result.Properties["SAMAccountName"][0] as string;

                        if (result.Properties["extensionAttribute1"].Count > 0)
                            item.CodiceFiscale = result.Properties["extensionAttribute1"][0] as string;

                        foreach (var property in result.Properties["mail"])
                            if (!String.IsNullOrEmpty(property as string))
                                item.Email.Add(property as string);

                        foreach (var property in result.Properties["telephoneNumber"])
                            if (!String.IsNullOrEmpty(property as string))
                                item.TelefonoServizioFisso.Add(property as string);

                        foreach (var property in result.Properties["mobile"])
                            if (!String.IsNullOrEmpty(property as string))
                                item.TelefonoServizioMobile.Add(property as string);

                        // Aggiungi l'elemento solo se ha almeno un contatto presente
                        if (item.Email.Any() || item.TelefonoServizioFisso.Any() || item.TelefonoServizioMobile.Any())
                            output.Add(item);
                    }
                }
            }

            return output;
        }

        internal List<GetLdapUserInfoByAccountNameOutputDto> GetLdapUserInfoByAccountName(GetLdapUserInfoByAccountNameInputDto dto)
        {
            if (String.IsNullOrEmpty(dto.SamAccountName))
                throw new ArgumentNullException(nameof(dto.SamAccountName));

            string domainName = null;
            string samAccountName = dto.SamAccountName;

            // Se il samAccountName fornito contiene una indicazione relativa al dominio, ignoriamola
            string[] accountNameElements = dto.SamAccountName.Split(new[] { '\\' }, 2);
            if (accountNameElements.Length > 1)
            {
                domainName = accountNameElements[0];
                samAccountName = accountNameElements[1];
            }

            List<GetLdapUserInfoByAccountNameOutputDto> output = new List<GetLdapUserInfoByAccountNameOutputDto>();
            
            DirectoryEntry searchRoot = new DirectoryEntry(ActiveDirectoryLdapUrl);

            using (DirectorySearcher search = new DirectorySearcher(searchRoot))
            {
                if (!String.IsNullOrEmpty(dto.LdapUsersFilter))
                    search.Filter = String.Format("(&" + dto.LdapUsersFilter + "(sAMAccountName={0}))", samAccountName);
                else
                    search.Filter = String.Format("(sAMAccountName={0})", samAccountName);

                search.PropertiesToLoad.Add("sAMAccountName");
                search.PropertiesToLoad.Add("sn");
                search.PropertiesToLoad.Add("givenName");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("extensionAttribute1");
                search.PropertiesToLoad.Add("company");
                search.PropertiesToLoad.Add("physicalDeliveryOfficeName");
                search.PropertiesToLoad.Add("telephoneNumber");
                search.PropertiesToLoad.Add("mobile");
                search.PropertiesToLoad.Add("manager");
                search.PageSize = 1000;

                var resultCol = SafeFindAll(search);

                if (resultCol != null)
                {
                    foreach (var result in resultCol)
                    {
                        GetLdapUserInfoByAccountNameOutputDto item = new GetLdapUserInfoByAccountNameOutputDto();

                        if (result.Properties["sAMAccountName"].Count > 0)
                            item.SamAccountName = result.Properties["sAMAccountName"][0] as string;

                        if (result.Properties["givenName"].Count > 0)
                            item.Nome = result.Properties["givenName"][0] as string;

                        if (result.Properties["sn"].Count > 0)
                            item.Cognome = result.Properties["sn"][0] as string;

                        if (result.Properties["extensionAttribute1"].Count > 0)
                            item.CodiceFiscale = result.Properties["extensionAttribute1"][0] as string;

                        if (result.Properties["company"].Count > 0)
                            item.Direzione = result.Properties["company"][0] as string;

                        if (result.Properties["physicalDeliveryOfficeName"].Count > 0)
                            item.Divisione = result.Properties["physicalDeliveryOfficeName"][0] as string;

                        if (result.Properties["manager"].Count > 0)
                            item.Manager = result.Properties["manager"][0] as string;

                        if (!String.IsNullOrEmpty(item.Manager))
                        {
                            DirectoryEntry managerEntry = new DirectoryEntry("LDAP://" + item.Manager);
                            if (managerEntry != null && managerEntry.Properties["sAMAccountName"] != null)
                            {
                                item.ManagerSamAccountName = (string)managerEntry.Properties["sAMAccountName"].Value;
                            }
                        }

                        foreach (var property in result.Properties["mail"])
                            if (!String.IsNullOrEmpty(property as string))
                                item.Email.Add(property as string);

                        foreach (var property in result.Properties["telephoneNumber"])
                            if (!String.IsNullOrEmpty(property as string))
                                item.TelefonoServizioFisso.Add(property as string);

                        foreach (var property in result.Properties["mobile"])
                            if (!String.IsNullOrEmpty(property as string))
                                item.TelefonoServizioMobile.Add(property as string);

                        output.Add(item);
                    }
                }
            }

            return output;
        }

        public GetUserInfoByAccountNameOutputDto GetUserInfoByAccountName(GetUserInfoByAccountNameInputDto dto)
        {
            if (String.IsNullOrEmpty(dto.SamAccountName))
                throw new ArgumentNullException(nameof(dto.SamAccountName));

            GetUserInfoByAccountNameOutputDto output = new GetUserInfoByAccountNameOutputDto();

            string domainName = null;
            string samAccountName = dto.SamAccountName;

            // Se il samAccountName fornito contiene una indicazione relativa al dominio, utilizzala
            string[] accountNameElements = dto.SamAccountName.Split(new[] { '\\' }, 2);
            if (accountNameElements.Length > 1)
            {
                domainName = accountNameElements[0];
                samAccountName = accountNameElements[1];
            }

            if (String.IsNullOrEmpty(samAccountName))
                throw new ArgumentNullException(nameof(dto.SamAccountName));

            using (var principalContext = new PrincipalContext(ContextType.Domain, domainName))
            {
                var principal = UserPrincipal.FindByIdentity(principalContext, samAccountName);
                if (principal == null)
                    return null;

                output.SamAccountName = principal.SamAccountName;
                output.Cognome = principal.Surname;
                output.Nome = principal.GivenName;
                output.DisplayName = principal.DisplayName;
                output.Email = principal.EmailAddress;
            }

            return output;
        }

        private IEnumerable<SearchResult> SafeFindAll(DirectorySearcher searcher)
        {
            using (SearchResultCollection results = searcher.FindAll())
            {
                foreach (SearchResult result in results)
                {
                    yield return result;
                }
            }   // SearchResultCollection will be disposed here
        }
    }
}
