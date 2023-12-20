namespace Dal;
using DO;

//stage 3
sealed internal class DalXml : IDal
{
    public ITask Task => new TaskImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public IDependency Dependency => new DependencyImplementation();
    public DateTime StartDate
    { get => Config.EndProjectDate; set => Config.EndProjectDate = value; }
    public DateTime EndDate
    { get => Config.EndProjectDate; set => Config.EndProjectDate = value; }
    public void Reset()
    {
        XMLTools.ResetFile("engineers");
        XMLTools.ResetFile("tasks");
        XMLTools.ResetFile("Dependencies");
    }
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
}
