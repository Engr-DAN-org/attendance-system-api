using System;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace api.Authorization;

public class OwnerOrRoleHandler(ILogger<OwnerOrRoleHandler> logger) : AuthorizationHandler<OwnerOrRoleRequirement>
{
    private readonly ILogger<OwnerOrRoleHandler> _logger = logger;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerOrRoleRequirement requirement)
    {
        // Get the user's role
        var roles = context.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier); // Use ClaimTypes.NameIdentifier for "sub"

        _logger.LogInformation("User roles: {Roles}", string.Join(", ", roles));
        _logger.LogInformation("User ID claim: {UserIdClaim}", userIdClaim?.Value);

        if (roles.Contains(UserRole.Teacher.ToString()) || roles.Contains(UserRole.Admin.ToString()))
        {
            // User is a Teacher or Admin -> Grant access
            _logger.LogInformation("Access granted: User is a Teacher or Admin");
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
                _logger.LogInformation("Requested user ID: {RequestedUserId}", requestedUserId);

                if (requestedUserId == userId)
                {
                    // User is accessing their own data -> Grant access
                    _logger.LogInformation("Access granted: User is accessing their own data");
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
        }

        // If none of the conditions match, deny access
        _logger.LogInformation("Access denied: None of the conditions matched");
        return Task.CompletedTask;
    }
}
