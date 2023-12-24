namespace BO;
//enums

/// <summary>
/// list of values ​​that represent different levels of experience for engineers.
/// </summary>
public enum EngineerExperience
{
    Novice,
    AdvancedBeginner,
    Competent,
    Proficient,
    Expert
}

/// <summary>
/// list of values that represents stat of task or milestone
/// </summary>
public enum Status
{
    Unscheduled,
    Scheduled,
    OnTrack,
    InJeopardy
}

/// <summary>
/// list of values that represents choice of user for action on entity
/// </summary>
public enum CRUD
{
    Create,
    Read,
    ReadAll,
    Update,
    Delete
}