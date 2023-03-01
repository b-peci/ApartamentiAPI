namespace Host.HelperMethods;

public class ClaimFunctionality
{
    public static Guid GetUserIdFromClaim()
    {
        string? userId = new HttpContextAccessor().HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "UserId")
            ?.Value;
        return string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
    }

    public static string? GetFullNameFromClaims()
    {
        string? fullName =
            new HttpContextAccessor().HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "displayName")?.Value;
        return fullName;
    }
}