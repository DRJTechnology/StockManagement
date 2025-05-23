using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StockManagement.Models.Dto.Profile;
using System.Security.Claims;

namespace StockManagement.UserManagement
{
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public CustomUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("FullNameClaimType", user.FullName));
            identity.AddClaim(new Claim("FirstNameClaimType", user.FirstName));
            return identity;
        }
    }

}