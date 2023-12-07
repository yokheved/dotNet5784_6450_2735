namespace Dal;
using DO;

//stage 3
sealed internal class DalXml : IDal
{
    public ITask Task => new TaskImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public IDependency Dependency => new DependencyImplementation();
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
}
