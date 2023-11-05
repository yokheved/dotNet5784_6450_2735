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
{
    #region variables
    int Id;
    String? Name;
    String? Email;
    EngineerExperience? Level;
    double? Cost;
    #endregion

    #region Ctors
    public Engineer()
    {
        /// <summary>
        /// Engineer record empty ctor, default id==0
        /// </summary>
        Id = 0;
    }

    public Engineer(
        int id,
        String? Name,
        String? Email,
        EngineerExperience? Level,
        double? Cost)
    {
        /// <summary>
        /// Engineer Entity represents a Engineer with all its props
        /// </summary>
        /// <param name="Id">Personal unique ID of the Engineer (as in national id card)</param>
        /// <param name="Name">Full Name of the Engineer</param>
        /// <param name="Email"></param>
        /// <param name="Level"></param>
        /// <param name="Cost"></param>
        Id = id;
    }
    #endregion
}