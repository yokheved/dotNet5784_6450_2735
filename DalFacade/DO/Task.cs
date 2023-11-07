namespace DO;
/// <summary>
/// Task record
/// </summary>
/// <param name="Id">Config uniuqe automatic running number for task</param>
/// <param name="Discription">Discription of task</param>
/// <param name="Alias">nicname</param>
/// <param name="IsMilestone">soon...</param>
/// <param name="CreatedAtDate"></param>
/// <param name="StartDate"></param>
/// <param name="ScheduledDate"></param>
/// <param name="ForecastDate"></param>
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
    bool? IsMilestone,
    DateTime? CreatedAtDate,
    DateTime StartDate,
    DateTime ScheduledDate,
    DateTime ForecastDate,
    DateTime DeadlineDate,
    DateTime CompleteDate,
    String? Deliverables,
    String? Remarks,
    int? EngineerId,
    EngineerExperience? ComplexityLevel = EngineerExperience.Novice
);