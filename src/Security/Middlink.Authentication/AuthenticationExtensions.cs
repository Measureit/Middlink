using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Middlink.Authentication
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddPermissionsSystem<TUserModel, TRoleModel>(this IServiceCollection services)
            where TUserModel : IdentityUser
            where TRoleModel : IdentityRole<string>
        {
            services.AddSingleton<IAuthorizationPolicyProvider, MiddlinkPermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler<TUserModel, TRoleModel>>();
            return services;
        }
    }
}
