using System.Reflection;
namespace BO;
/// <summary>
/// milestone in list: description, alias, create at date,
/// status, completion percentage - all properties
/// </summary>
public class MilestoneInList
{
    public string? Description { get; init; }
    public string? Alias { get; init; }
    public DateTime CreatedAtDate { get; init; }
    public Status Status { get; set; }
    public double CompletionPercentage { get; set; }
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
