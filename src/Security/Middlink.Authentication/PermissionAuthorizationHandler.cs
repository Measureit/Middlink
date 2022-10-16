using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Middlink.Authentication
{
    internal class PermissionAuthorizationHandler<TUserModel, TRoleModel> : AuthorizationHandler<PermissionRequirement> 
        where TUserModel : IdentityUser
        where TRoleModel : IdentityRole<string>
    {
        private readonly UserManager<TUserModel> _userManager;
        private readonly RoleManager<TRoleModel> _roleManager;

        public PermissionAuthorizationHandler(UserManager<TUserModel> userManager, RoleManager<TRoleModel> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User != null)
            {
                var user = await _userManager.GetUserAsync(context.User);
                if (user == null)
                {
                    return;
                }
                var userRoleNames = await _userManager.GetRolesAsync(user);
                var userRoles = _roleManager.Roles.Where(x => userRoleNames.Contains(x.Name)).ToList();

                foreach (var role in userRoles)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    var permissions = roleClaims.Where(x => x.Type == CustomClaimTypes.Permission &&
                                                            x.Value == requirement.Permission
                                                            //&& x.Issuer == "LOCAL AUTHORITY"
                                                            )
                                                .Select(x => x.Value);

                    if (permissions.Any())
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
}
