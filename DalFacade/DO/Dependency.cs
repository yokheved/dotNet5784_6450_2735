namespace DO;

/// <summary>
/// Dependency Entity represents a Dependency with all its props
/// </summary>
/// <param name="Id">Config uniuqe automatic running number for Dependency</param>
/// <param name="DependentTask">id of this dependent task</param>
/// <param name="DependsOnTask">id of the last task this is dependent on</param>
public record Dependency
(
    int Id,
    int DependentTask,
    int DependsOnTask
);
