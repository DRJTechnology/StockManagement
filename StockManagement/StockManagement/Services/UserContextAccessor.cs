using System.Security.Claims;

public interface IUserContextAccessor
{
    int? GetCurrentUserId();
}

public class UserContextAccessor : IUserContextAccessor
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public int? GetCurrentUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                return userId;
        }
        return null;
    }
   }