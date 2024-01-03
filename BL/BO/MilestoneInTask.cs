namespace BO;
/// <summary>
/// milestone in task: id, alias - all properties
/// </summary>
public class MilestoneInTask
{
    public int Id { get; init; }
    public string? Alias { get; init; }
    /// <summary>
    /// converts object to name of object and properties
    /// </summary>
    /// <returns> a string like: "objType:\n propName: propVal\n.....</returns>
    public override string ToString() => Tools<MilestoneInTask>.PrintProperties(this);
}
