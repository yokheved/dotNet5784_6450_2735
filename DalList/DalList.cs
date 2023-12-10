namespace Dal;
using DO;

sealed internal class DalList : IDal
{
    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();
    public DateTime? StartDate => new DateTime?();
    public DateTime? EndDate => new DateTime?();
    public void Reset()
    {
        DataSource.Engineers.Clear();
        DataSource.Tasks.Clear();
        DataSource.Dependencies.Clear();
    }
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
}
