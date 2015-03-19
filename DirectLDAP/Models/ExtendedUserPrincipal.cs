using System.DirectoryServices.AccountManagement;

namespace DirectLDAP.Models
{
    [DirectoryRdnPrefix("CN")]
    [DirectoryObjectClass("User")]
    public class ExtendedUserPrincipal : UserPrincipal
    {
        private const string AdminDescriptionProperty = "adminDescription";
        public ExtendedUserPrincipal(PrincipalContext context) 
            : base(context)
        {            
        }

        public ExtendedUserPrincipal(PrincipalContext context, string samAccountName, string password, bool enabled)
            : base(context, samAccountName, password, enabled)
        {
        }

        [DirectoryProperty(AdminDescriptionProperty)]
        public string AdminDescription
        {
            get
            {
                var result = ExtensionGet(AdminDescriptionProperty);

                if (result == null || result.Length != 1)
                {
                    return null;
                }

                return (string)result[0];

            }
            set { this.ExtensionSet(AdminDescriptionProperty, value); }
        }

        public static new ExtendedUserPrincipal FindByIdentity(PrincipalContext context, string identityValue)
        {
            return (ExtendedUserPrincipal)FindByIdentityWithType(context, typeof(ExtendedUserPrincipal), identityValue);
        }

        public static new ExtendedUserPrincipal FindByIdentity(PrincipalContext context, IdentityType identityType, string identityValue)
        {
            return (ExtendedUserPrincipal)FindByIdentityWithType(context, typeof(ExtendedUserPrincipal), identityType, identityValue);
        }
    }
}