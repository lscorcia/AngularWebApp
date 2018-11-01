namespace AngularWebApp.Infrastructure.ActiveDirectory.Models
{
    public class GetLdapUserInfoByAccountNameInputDto
    {
        public string SamAccountName { get; set; }
        public string LdapUsersFilter { get; set; }
    }
}