using System;

namespace api.Exceptions;

public class NotFoundException(string modelName, object key) : AppException($"{modelName} with an ID of {key} Does not Exist.", 404)
{
}

