using System.Reflection;

namespace BO;

public class Engineer
{
    int Id { get; init; }
    string? Name { get; init; }
    string? Email { get; init; }
    EngineerExperience Level { get; set; }
    double Cost { get; set; }
    TaskInEngineer? Task { get; set; }
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

