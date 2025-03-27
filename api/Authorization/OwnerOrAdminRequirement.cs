using System;
using Microsoft.AspNetCore.Authorization;

namespace api.Authorization;

public class OwnerOrAdminRequirement : IAuthorizationRequirement
{

}
