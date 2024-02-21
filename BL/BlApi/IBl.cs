namespace BlApi;

public interface IBl
{
    static public IEngineer? Engineer { get; }
    static public ITask? Task { get; }
    public IMilestone Milestone { get; }

    public void Reset(string? entity = "");
}
