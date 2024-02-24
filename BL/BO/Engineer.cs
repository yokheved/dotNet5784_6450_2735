namespace BO;
/// <summary>
/// engineer: id, name, email, level, cost, task(now) - all properties
/// </summary>
public class Engineer
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public EngineerExperience? Level { get; set; }
    public double Cost { get; set; }
    public TaskInEngineer? Task { get; set; }
    public override string ToString() => Tools<Engineer>.PrintProperties(this);
}
