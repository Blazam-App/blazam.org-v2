using System.Security.Claims;

namespace blazam.org.Helpers
{
    public static class ClaimsPrincipalHelpers
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            if (user == null || user.Identity?.IsAuthenticated != true)
            {
                throw new InvalidOperationException("User is not authenticated.");
            }
            var userIdClaim = user.FindFirst(ClaimTypes.Sid);
            if (userIdClaim == null)
            {
                throw new InvalidOperationException("User ID claim not found.");
            }
            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new InvalidOperationException("User ID claim is not a valid integer.");
            }
            return userId;
        }
    }
}
