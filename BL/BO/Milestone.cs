using System.Reflection;
namespace BO;
/// <summary>
/// milestone: id, alias, create at date, status, start at date,
/// approx end at date, last date to end, end at date, completion percentage,
/// remarks, dependencies list. - all properties
/// </summary>
public class Milestone
{
    public int Id { get; init; }
    public string? Alias { get; init; }
    public DateTime CreateAtDate { get; init; }
    public Status Status { get; init; }
    public DateTime StartAtDate { get; set; }
    public DateTime ApproxEndAtDate { get; set; }
    public DateTime LastDateToEnd { get; set; }
    public DateTime? EndAtDate { get; set; }
    public double CompletionPercentage { get; set; }
    public string? Remarks { get; set; }
    public List<TaskInList>? DependenciesList { get; set; }
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
