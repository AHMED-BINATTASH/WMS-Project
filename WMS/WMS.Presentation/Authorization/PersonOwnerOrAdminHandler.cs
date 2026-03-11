using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
public class PersonOwnerOrAdminHandler
    : AuthorizationHandler<PersonOwnerOrAdminRequirement, int>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PersonOwnerOrAdminRequirement requirement,
        int studentId)
    {
        if (context.User.IsInRole("Admin"))
        {
            // Mark the requirement as satisfied
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (int.TryParse(userId, out int authenticatedStudentId) &&
            authenticatedStudentId == studentId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
