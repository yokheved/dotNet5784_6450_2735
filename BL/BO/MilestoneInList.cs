namespace BO;
/// <summary>
/// milestone in list: id, description, alias, create at date,
/// status, completion percentage - all properties
/// </summary>
public class MilestoneInList
{
    public int Id { get; init; }
    public string? Description { get; init; }
    public string? Alias { get; init; }
    public DateTime CreatedAtDate { get; init; }
    public Status Status { get; set; }
    public double CompletionPercentage { get; set; }
    /// <summary>
    /// converts object to name of object and properties
    /// </summary>
    /// <returns> a string like: "objType:\n propName: propVal\n.....</returns>
    public override string ToString() => Tools<MilestoneInList>.PrintProperties(this);
}
