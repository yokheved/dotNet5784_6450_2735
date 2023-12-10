namespace BlApi;
/// <summary>
/// task interface namespace BO
/// </summary>
public interface ITask
{
    /// <summary>
    /// get list of tasks - all or filtered
    /// </summary>
    /// <param name="filter">optional function for filtering wich recives a BO.Enginner and returns bool</param>
    /// <returns>IEnumerable for list of tasks, all or filtered</returns>
    public IEnumerable<BO.Task> GetTasks(Func<BO.Task, bool>? filter = null);
    /// <summary>
    /// gets from DB task with param id, if does not exist throws an error
    /// </summary>
    /// <param name="id">id for task to get from DB</param>
    /// <returns>BO.Task with id as param</returns>
    public BO.Task GetTask(int id);
    /// <summary>
    /// adds param task as a new task to DB if exists throws an error
    /// </summary>
    /// <param name="task">task to add to DB</param>
    public void AddTask(BO.Task task);
    /// <summary>
    /// updates task with id as given task with given task
    /// - if does not exist throws an error
    /// </summary>
    /// <param name="task">updated task to update in DB</param>
    public void UpdateTask(BO.Task task);
    /// <summary>
    /// deletes task with param id from DB, iff no task is dependent on it
    /// - if is, throws an error
    /// </summary>
    /// <param name="id">id of task to delete</param>
    public void DeleteTask(int id);
}
