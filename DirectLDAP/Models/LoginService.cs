using System.DirectoryServices.AccountManagement;

namespace DirectLDAP.Models
{
    public class LoginService
    {
        public bool IsValid(string username, string password)
        {
            var principalContext = ConnectToActiveDirectory();    
     
            return principalContext.ValidateCredentials(username, password);
        }

        public ActiveDirectoryUser FindUserById(string username)
        {
            var principalContext = ConnectToActiveDirectory();            
            //var userPrincipal = UserPrincipal.FindByIdentity(principalContext, username);
            var userPrincipal = ExtendedUserPrincipal.FindByIdentity(principalContext, username);
            if (userPrincipal == null)
            {
                return null;
            }

            return new ActiveDirectoryUser
            {
                GivenName = userPrincipal.GivenName,
                Surname = userPrincipal.Surname,
                Email = userPrincipal.EmailAddress
            };
        }

        private PrincipalContext ConnectToActiveDirectory()
        {
            return new PrincipalContext(ContextType.Domain,
                ApplicationSettings.ActiveDirectoryServer,
                "ou=Samples,dc=david,dc=2359media,dc=com",
                ApplicationSettings.ActiveDirectoryUsername,
                ApplicationSettings.ActiveDirectoryPassword);            
        }
    }
}