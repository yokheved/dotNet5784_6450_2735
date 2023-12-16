namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
}

[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
}

[Serializable]
public class BlDeletionImpossible : Exception
{
    public BlDeletionImpossible(string? message) : base(message) { }
}

[Serializable]
public class BlIsInTheMiddleOfTask : Exception
{
    public BlIsInTheMiddleOfTask(string? message) : base(message) { }
}

[Serializable]
public class BlNotValidValueExeption : Exception
{
    public BlNotValidValueExeption(string? message) : base(message) { }
}

[Serializable]
public class BlIsADependencyExeption : Exception
{
    public BlIsADependencyExeption(string? message) : base(message) { }
}