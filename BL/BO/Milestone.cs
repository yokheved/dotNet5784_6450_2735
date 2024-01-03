namespace BO;
/// <summary>
/// milestone: id, alias, create at date, status, start at date,
/// approx end at date, last date to end, end at date, completion percentage,
/// remarks, dependencies list. - all properties
/// </summary>
public class Milestone
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public DateTime CreateAtDate { get; init; }
    public Status Status { get; init; }
    public DateTime StartAtDate { get; set; }
    public DateTime ApproxStartAtDate { get; set; }
    public DateTime LastDateToEnd { get; set; }
    public DateTime? EndAtDate { get; set; }
    public double CompletionPercentage { get; set; }
    public string? Remarks { get; set; }
    public List<TaskInList>? DependenciesList { get; set; }
    /// <summary>
    /// converts object to name of object and properties
    /// </summary>
    /// <returns> a string like: "objType:\n propName: propVal\n.....</returns>
    public override string ToString() => Tools<Milestone>.PrintProperties(this);
}
