namespace DO;
/// <summary>
/// Engineer Entity represents a Engineer with all its props
/// </summary>
/// <param name="Id">Personal unique ID of the Engineer (as in national id card)</param>
/// <param name="Name">Full Name of the Engineer</param>
/// <param name="Email"></param>
/// <param name="Level"></param>
/// <param name="Cost"></param>

public record Engineer
(
    int Id,
    String? Name,
    String? Email,
    EngineerExperience? Level,
    double? Cost
)
{
    public Engineer() : this(default, default, default, default, default) { }
};