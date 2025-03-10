using System;

namespace api.Exceptions;

public class UnauthorizedException(string message = "Unauthorized Access.") : AppException(message, 401)
{

}
