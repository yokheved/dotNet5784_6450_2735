using System.Reflection;
namespace BO;
/// <summary>
/// task: id, description, alias, create at date, approx start at date, 
/// start at date, last date to end, approx end at date, status, dependencies list,
/// milestone, deliverables, remarks, engineer, level - all properties
/// </summary>
public class Task
{
    public int Id { get; init; }
    public String? Description { get; init; }
    public String? Alias { get; init; }
    public TimeSpan? Duration { get; init; }
    public DateTime CreatedAtDate { get; init; }
    public DateTime? ApproxStartAtDate { get; set; }
    public DateTime? StartAtDate { get; set; }
    public DateTime? LastDateToEnd { get; set; }
    public DateTime? EndAtDate { get; set; }
    public Status Status { get; set; }
    public List<TaskInList>? DependenciesList { get; set; }
    public MilestoneInTask? Milestone { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public EngineerExperience? Level { get; set; }
    /// <summary>
    /// converts object to name of object and properties
    /// </summary>
    /// <returns> a string like: "objType:\n propName: propVal\n.....</returns>
    public override string ToString()
    {
        Type type = this.GetType();//reflection
        PropertyInfo[] properties = type.GetProperties();
        string result = $"{type.Name} Properties:\n";
        foreach (PropertyInfo property in properties)
        {
            object? value = property.GetValue(this);
            result += value != null ? $"{property.Name}: {value.ToString()}\n" : "";
        }

        return result;
    }
}
