using System.Reflection;

namespace BO;

public class Milestone
{
    int Id { get; init; }
    string? Alias { get; init; }
    DateTime CreateAtDate { get; init; }
    Status Status { get; init; }
    DateTime StartAtDate { get; set; }
    DateTime ApproxEndAtDate { get; set; }
    DateTime LastDateToEnd { get; set; }
    DateTime EndAtDate { get; set; }
    double CompletionPercentage { get; set; }
    string? Remarks { get; set; }
    List<TaskInList>? DependenciesList { get; set; }
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
