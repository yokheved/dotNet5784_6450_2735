namespace DO;

/// <summary>
/// Dependency Entity represents a Dependency with all its props
/// </summary>
/// <param name="Id">Config uniuqe automatic running number for Dependency</param>
/// <param name="DependentTask">id of this dependent task</param>
/// <param name="DependsOnTask">id of the last task this is dependent on</param>
public record Dependency
{
    #region variables
    int Id;
    int DependentTask;
    int DependsOnTask;
    #endregion

    #region Ctors
    public Dependency()
    {
        /// <summary>
        /// Dependency record empty Ctors
        /// </summary>
        DependentTask = 0;
        Id = 0;//change to Config
        DependsOnTask = 0;
    }

    public Dependency(int Id, int DependentTask, int DependsOnTask)
    {
        /// <summary>
        /// Dependency record parameter Ctors
        /// </summary>
        /// <param name="Id">Config uniuqe automatic running number for Dependency</param>
        /// <param name="DependentTask">id of this dependent task</param>
        /// <param name="DependsOnTask">id of the last task this is dependent on</param>
        this.Id = Id;
        this.DependentTask = DependentTask;
        this.DependsOnTask = DependsOnTask;
    }
    #endregion
}
