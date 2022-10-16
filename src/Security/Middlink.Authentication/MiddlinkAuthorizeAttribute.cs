using Microsoft.AspNetCore.Authorization;

namespace Middlink.Authentication
{
    public class MiddlinkAuthorizeAttribute : AuthorizeAttribute
    {
        internal const string POLICY_PREFIX = "Middlink";

        public MiddlinkAuthorizeAttribute(string permission) => Permission = permission;

        public string Permission
        {
            get
            {
                return Policy.Substring(POLICY_PREFIX.Length);
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }
    }
}
