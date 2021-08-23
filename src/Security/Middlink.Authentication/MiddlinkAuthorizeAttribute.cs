using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

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
