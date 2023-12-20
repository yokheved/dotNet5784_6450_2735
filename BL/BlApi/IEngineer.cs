namespace BlApi;
/// <summary>
/// engineer interface namespace BO
/// </summary>
public interface IEngineer
{
    /// <summary>
    /// get a list of engineers all or filtered
    /// </summary>
    /// <param name="filter">optional function for filtering wich recives a BO.Enginner and returns bool</param>
    /// <returns>IEnumerable for  BO.Engineer, with all engineers or engineers that aswered the filter function</returns>
    public IEnumerable<BO.Engineer> GetEngineerList(Func<BO.Engineer, bool>? filter = null);
    /// <summary>
    /// gets from DB the enginneer to return, if does not exist throws an error
    /// </summary>
    /// <param name="id">id of engineer to return</param>
    /// <returns>BO.Engineer with param id</returns>
    public BO.Engineer GetEngineer(int id);
    /// <summary>
    /// deletes engineer with param id if not in middle of a task,
    /// if is in the middle or does not exist - throws an error
    /// </summary>
    /// <param name="id">id of engineer to delete</param>
    public void DeleteEgineer(int id);
    /// <summary>
    /// adds to the DB a new engineer, as given in param
    /// </summary>
    /// <param name="engineer">BO.Engineer to add to DB</param>
    public void AddEngineer(BO.Engineer engineer);
    /// <summary>
    /// updates engineer with id as in param, to be engineer as in param
    /// </summary>
    /// <param name="engineer">updated engineer to update in DB</param>
    public void UpdateEngineer(BO.Engineer engineer);
}
