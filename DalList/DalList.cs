namespace Dal;
using DO;
sealed internal class DalList : IDal
{
    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();
    public DateTime StartDate
    { get => DataSource.Config.EndProjectDate; set => DataSource.Config.EndProjectDate = value; }
    public DateTime EndDate
    { get => DataSource.Config.EndProjectDate; set => DataSource.Config.EndProjectDate = value; }
    public void Reset()
    {
        DataSource.Engineers.Clear();
        DataSource.Tasks.Clear();
        DataSource.Dependencies.Clear();
    }
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
}
