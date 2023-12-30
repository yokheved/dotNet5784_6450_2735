namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string? message, Exception ex) : base(message, ex) { }
}

[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string? message, Exception ex) : base(message, ex) { }
}

[Serializable]
public class BlDeletionImpossible : Exception
{
    public BlDeletionImpossible(string? message) : base(message) { }
    public BlDeletionImpossible(string? message, Exception ex) : base(message, ex) { }
}

[Serializable]
public class BlIsInTheMiddleOfTask : Exception
{
    public BlIsInTheMiddleOfTask(string? message) : base(message) { }
    public BlIsInTheMiddleOfTask(string? message, Exception ex) : base(message, ex) { }
}

[Serializable]
public class BlNotValidValueExeption : Exception
{
    public BlNotValidValueExeption(string? message) : base(message) { }
    public BlNotValidValueExeption(string? message, Exception ex) : base(message, ex) { }
}

[Serializable]
public class BlIsADependencyExeption : Exception
{
    public BlIsADependencyExeption(string? message) : base(message) { }
    public BlIsADependencyExeption(string? message, Exception ex) : base(message, ex) { }
}

[Serializable]
public class BlCirclingDependenciesExeption : Exception
{
    public BlCirclingDependenciesExeption(string? message) : base(message) { }
    public BlCirclingDependenciesExeption(string? message, Exception ex) : base(message, ex) { }
}
