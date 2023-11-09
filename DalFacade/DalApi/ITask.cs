namespace DO;

public interface ITask
{
    /// <summary>
    /// Creates new Task entity object in DAL
    /// </summary>
    /// <param name="item">a Task object to add to the list</param>
    /// <returns>id of task added</returns>
    int Create(Task item);
    /// <summary>
    /// Reads Task entity object by its ID 
    /// </summary>
    /// <param name="id">id of task to read</param>
    /// <returns>Task with id param id, if not found - null</returns>
    Task? Read(int id);
    /// <summary>
    /// Reads all Task entity objects
    /// </summary>
    /// <returns>list of all tasks</returns>
    List<Task> ReadAll();
    /// <summary>
    /// Updates task entity object
    /// </summary>
    /// <param name="item">task with id to update and values to update</param>
    void Update(Task item);
    /// <summary>
    /// Deletes a task object by its Id
    /// </summary>
    /// <param name="id">id of task to delete</param>
    void Delete(int id);

}
