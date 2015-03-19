using System.Configuration;

namespace DirectLDAP.Models
{
    public class ApplicationSettings
    {
        public static string ActiveDirectoryServer
        {
            get { return ConfigurationManager.AppSettings["ActiveDirectoryServer"]; }
        }

        public static string ActiveDirectoryUsername
        {
            get { return ConfigurationManager.AppSettings["ActiveDirectoryUsername"]; }
        }

        public static string ActiveDirectoryPassword
        {
            get { return ConfigurationManager.AppSettings["ActiveDirectoryPassword"]; }
        }
    }
}