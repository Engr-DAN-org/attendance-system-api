using System;

namespace api.Exceptions;

public class ValidationException(string message = "Validation Error Occured.") : AppException(message, 400)
{
}

