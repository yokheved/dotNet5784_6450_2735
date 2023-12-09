﻿using System.Reflection;

namespace BO;

public class MilestoneInList
{
    string? Description { get; init; }
    string? Alias { get; init; }
    DateTime CreatedAt { get; init; }
    Status Status { get; set; }
    double CompletionPercentage { get; set; }
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
