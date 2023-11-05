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
{
    #region Variables
    int Id;
    String? Discription;
    String? Alias;
    bool? IsMilestone;
    DateTime? CreatedAtDate;
    DateTime StartDate;
    DateTime ScheduledDate;
    DateTime ForecastDate;
    DateTime DeadlineDate;
    DateTime CompleteDate;
    String? Deliverables;
    String? Remarks;
    int? EngineerId;
    EngineerExperience? ComplexityLevel;
    #endregion

    #region Ctors
    public Task(int Id,
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
    EngineerExperience? ComplexityLevel = EngineerExperience.Novice)
    {
        /// <summary>
        /// Task record parameters Ctor
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
        /// <param name="ComplexityLevel">ComplexityLevel of the task ---default of Novice</param>
        this.Id = Id;//change to Config 
        //for all dates check correct chronological order
        this.CreatedAtDate = CreatedAtDate;
        this.StartDate = StartDate;
        this.ScheduledDate = ScheduledDate;
        this.ForecastDate = ForecastDate;
        this.DeadlineDate = DeadlineDate;
        this.CompleteDate = CompleteDate;
        this.EngineerId = EngineerId;//need to check value
    }
    public Task()
    {
        /// <summary>
        /// Task record empty ctor, sets only id as Config
        /// </summary>

        Id = 0;//change to Config
    }
    #endregion
}
