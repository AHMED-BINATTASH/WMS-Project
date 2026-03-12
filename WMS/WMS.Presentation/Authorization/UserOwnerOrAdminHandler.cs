using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
public class UserOwnerOrAdminHandler
    : AuthorizationHandler<UserOwnerOrAdminRequirement, int>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserOwnerOrAdminRequirement requirement,
        int UserID)
    {
        if (context.User.IsInRole("Admin"))
        {
            // Mark the requirement as satisfied
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (int.TryParse(userId, out int authenticatedUserId) &&
            authenticatedUserId == UserID)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
