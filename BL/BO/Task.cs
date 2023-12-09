using System.Reflection;

namespace BO;

public class Task
{
    int id { get; init; }
    String? Description { get; init; }
    String? Alias { get; init; }
    DateTime CreatedAtDate { get; init; }
    DateTime ApproxStartAtDate { get; set; }
    DateTime StartAtDate { get; set; }
    DateTime LastDateToEnd { get; set; }
    DateTime ApproxEndAtDate { get; set; }
    Status Status { get; set; }
    List<TaskInList>? DependenciesList { get; set; }
    MilestoneInTask? Milestone { get; set; }
    string? Deliverables { get; set; }
    string? Remarks { get; set; }
    EngineerInTask? Engineer { get; set; }
    EngineerExperience? Level { get; set; }
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
