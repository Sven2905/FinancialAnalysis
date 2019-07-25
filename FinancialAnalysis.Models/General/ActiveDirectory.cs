using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace FinancialAnalysis.Models.General
{
    public static class ActiveDirectory
    {
        public static bool IsAuthenticated(string domain, string username, string pwd)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
            {
                // validate the credentials
                return pc.ValidateCredentials(username, pwd);
            }
        }

        /// <summary>
        /// Checks if user exists in the active directory
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool DoesUserExist(string userName, string domain)
        {
            using (var domainContext = new PrincipalContext(ContextType.Domain, domain))
            {
                using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, userName))
                {
                    return foundUser != null;
                }
            }
        }

        public static List<string> ListAllUser()
        {
            List<string> usernames = new List<string>();
            DirectoryEntry directoryEntry = new DirectoryEntry
                    ("WinNT://" + Environment.UserDomainName);
            string authenticationType = "";
            foreach (DirectoryEntry child in directoryEntry.Children)
            {
                if (child.SchemaClassName == "User")
                {
                    usernames.Add(child.Name);
                    authenticationType += child.Username + Environment.NewLine;
                }
            }
            return usernames;
        }

        public static UserPrincipal GetUserInformation(string userName, string domain)
        {
            using (var domainContext = new PrincipalContext(ContextType.Domain, domain))
            {
                using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, userName))
                {
                    return foundUser;
                }
            }
        }
    }
}