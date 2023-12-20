namespace BlApi;
/// <summary>
/// milestone interface namespace BO
/// </summary>
public interface IMilestone
{
    /// <summary>
    /// gets milstone with id as param
    /// </summary>
    /// <param name="id">id of milstone to get</param>
    /// <returns>milestone with id as param</returns>
    public BO.Milestone GetMilestone(int id);
    /// <summary>
    /// updates milestone with id as param
    /// </summary>
    /// <param name="id">id of milstone to update</param>
    /// <returns>updated milestone</returns>
    public BO.Milestone UpdateMilestone(int id);
}
