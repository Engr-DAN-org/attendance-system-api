using System;

namespace api.Exceptions;

public class ServerException(string message = "Internal Server Error.") : AppException(message, 500)
{
}

