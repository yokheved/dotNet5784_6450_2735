using System.Collections;
using System.Reflection;

namespace BO;

public static class Tools<T>
{
    public static string PrintProperties(T obj)
    {
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();
        string result = $"{type.Name} Properties:\n";

        foreach (PropertyInfo property in properties)
        {
            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
            {
                result += $"{property.Name}:\n";
                IEnumerable? enumerableValue = property.GetValue(obj) as IEnumerable;

                if (enumerableValue != null && enumerableValue.GetEnumerator().MoveNext()) // Check for non-empty enumerable
                {
                    foreach (var item in enumerableValue)
                    {
                        result += $"\t{item}\n";
                    }
                }
                else
                {
                    result += "\n";
                }
            }
            else if (property.PropertyType == typeof(DateTime) && property != null)
            {
                DateTime dateTimeValue = (DateTime)property.GetValue(obj);
                if (dateTimeValue == DateTime.MinValue)
                {
                    result += $"{property.Name}: DateTime.MinValue\n";
                }
                else
                {
                    result += $"{property.Name}: {dateTimeValue}\n";
                }
            }
            else if (property != null) // For non-enumerable properties
            {
                object? value = property.GetValue(obj);
                result += value != null ? $"{property.Name}: {value.ToString()}\n" : "";
            }
        }

        return result;
    }
}

