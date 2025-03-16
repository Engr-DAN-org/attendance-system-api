using System;

namespace api.Exceptions;

public class NotFoundException(string modelName) : AppException($"{modelName} does not Exist.", 404)
{
}

