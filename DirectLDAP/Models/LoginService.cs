using System;
using System.DirectoryServices.AccountManagement;

namespace DirectLDAP.Models
{
    public class LoginService
    {
        public bool IsValid(string username, string password)
        {
            return Execute(context =>
            {
                return context.ValidateCredentials(username, password);
            });
        }

        public ActiveDirectoryUser FindUserById(string username)
        {            
            //var userPrincipal = Execute(context => UserPrincipal.FindByIdentity(context, username));
            var userPrincipal = Execute(context => ExtendedUserPrincipal.FindByIdentity(context, username));
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

        private T Execute<T>(Func<PrincipalContext, T> func)
        {
            using (var principalContext = ConnectToActiveDirectory())
            {
                return func(principalContext);
            }
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