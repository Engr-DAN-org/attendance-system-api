using System;
using System.Security.Claims;
using api.Enums;
using Microsoft.AspNetCore.Authorization;

namespace api.Authorization;

public class RequireSelfOrRoleHandler : AuthorizationHandler<RequireSelfOrRoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequireSelfOrRoleRequirement requirement)
    {
        // Get the user's role
        var roles = context.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier); // Use ClaimTypes.NameIdentifier for "sub"

        if (roles.Contains(UserRole.Teacher.ToString()) || roles.Contains(UserRole.Admin.ToString()))
        {
            // User is a Teacher or Admin -> Grant access
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (userIdClaim != null)
        {
            var userId = userIdClaim.Value;

            // Get the resource being accessed
            if (context.Resource is HttpContext httpContext && httpContext.Request.RouteValues.TryGetValue("id", out var routeValue))
            {
                var requestedUserId = routeValue?.ToString();
                if (requestedUserId == userId)
                {
                    // User is accessing their own data -> Grant access
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
        }

        // If none of the conditions match, deny access
        return Task.CompletedTask;
    }
}
