using System;
using Microsoft.AspNetCore.Authorization;

namespace api.Authorization;

public class RequireSelfOrRoleRequirement : IAuthorizationRequirement
{

}
