namespace BO;
/// <summary>
/// task in list; id, description, alias, status - all properties
/// </summary>
public class TaskInList
{
    public int Id { get; init; }
    public string? Description { get; init; }
    public string? Alias { get; init; }
    public Status Status { get; set; }
    /// <summary>
    /// converts object to name of object and properties
    /// </summary>
    /// <returns> a string like: "objType:\n propName: propVal\n.....</returns>
    public override string ToString() => Tools<TaskInList>.PrintProperties(this);
}
