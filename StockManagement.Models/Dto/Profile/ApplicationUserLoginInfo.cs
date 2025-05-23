using Microsoft.AspNetCore.Identity;

namespace StockManagement.Models.Dto.Profile
{
    public class ApplicationUserLoginInfo : UserLoginInfo
    {
        public ApplicationUserLoginInfo(string loginProvider, string providerKey, string? displayName)
            : base(loginProvider, providerKey, displayName)
        {
        }

        public ApplicationUserLoginInfo()
            : base(string.Empty, string.Empty, null)
        {
        }
    }
}
