using Microsoft.AspNetCore.Identity;

namespace StockManagement.Models.Dto.Profile
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName
        {
            get
            {
                return (FirstName + " " + LastName).Trim();
            }
        }
    }
}
