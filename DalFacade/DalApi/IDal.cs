namespace DO;

public interface IDal
{
    IDependency? Dependency { get; }
    IEngineer? Engineer { get; }
    ITask? Task { get; }
    DateTime? StartDate { get; }
    DateTime? EndDate { get; }
    void Reset();
}
