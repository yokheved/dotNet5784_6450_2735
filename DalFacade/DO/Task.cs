namespace DO;
/// <summary>
/// Task record
/// </summary>
/// <param name="Id">Config uniuqe automatic running number for task</param>
/// <param name="Discription">Discription of task</param>
/// <param name="Alias">nicname</param>
/// <param name="IsMilestone">soon...</param>
/// <param name="CreatedAtDate"></param>
/// <param name="ScheduledDate">scheduled date to start task</param>
/// <param name="StartDate">actule start date</param>
/// <param name="DeadlineDate"></param>
/// <param name="CompleteDate"></param>
/// <param name="Deliverables">results of the task</param>
/// <param name="Remarks">comments on the task</param>
/// <param name="EngineerId">id of engineer working on the task</param>
/// <param name="ComplexityLevel">ComplexityLevel of the task</param>
public record Task
(
    int Id,
    String? Discription,
    String? Alias,
    bool IsMilestone,
    TimeSpan? Duration,
    DateTime CreatedAtDate,
    DateTime? ScheduledDate,
    DateTime? StartDate,
    DateTime? DeadlineDate,
    DateTime? CompleteDate,
    String? Deliverables,
    String? Remarks,
    int? EngineerId,
    EngineerExperience ComplexityLevel = EngineerExperience.Novice
)
{
    public Task() : this(default, default, default, default, default, default,
        default, default, default, default, default, default, default, default)
    { }
};