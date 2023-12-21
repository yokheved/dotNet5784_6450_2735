namespace Dal;
using DO;

internal class TaskImplementation : ITask
{
    /// <summary>
    /// Creates new Task entity object in DAL
    /// </summary>
    /// <param name="item">Task type</param>
    /// <returns>integer - new Task item id</returns>
    public int Create(Task item)
    {
        List<Task> list = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        if (list.Any(t => t == item))
        {
            throw new DalAlreadyExistsException($"Object of type Task with ID {item.Id} exists.");
        }
        int id = Config.NextTaskId;
        Task tempItem = new Task(id, item.Discription, item.Alias, item.IsMilestone, item.Duration,
            item.CreatedAtDate, item.StartDate, item.ScheduledDate, item.DeadlineDate,
            item.CompleteDate, item.Deliverables, item.Remarks, item.EngineerId, item.ComplexityLevel);
        list.Add(tempItem);
        XMLTools.SaveListToXMLSerializer<Task>(list, "tasks");
        return id;
    }
    /// <summary>
    /// Deletes a Task object by its Id
    /// </summary>
    /// <param name="id">id of Task to delete</param>
    public void Delete(int id)
    {
        List<Task> list = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? deleteIt = (from t in list
                          where t.Id == id
                          select t).ToList().FirstOrDefault();

        if (deleteIt == null)
        {
            throw new DalAlreadyExistsException($"Object of type Task with ID {id} does not exist.");
        }
        else
        {
            list.Remove(deleteIt);
        }
        XMLTools.SaveListToXMLSerializer<Task>(list, "tasks");
    }
    /// <summary>
    /// Reads Task entity object by its ID 
    /// </summary>
    /// <param name="id">id of Task to read</param>
    /// <returns>Task object with param id, if not found - null</returns>
    public Task Read(int id)
    {
        List<Task> list = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? task = (from t in list
                      where t.Id == id
                      select t).ToList().FirstOrDefault()
                      ?? throw new DalDoesNotExistException($"task with id {id} does not exist");
        return task;
    }

    public Task Read(Func<Task, bool> filter)
    {
        List<Task> list = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? task = list
           .FirstOrDefault(filter) ?? throw new DalDoesNotExistException($"task does not exist");
        return task;
    }
    /// <summary>
    /// Reads all entity objects by lambda function returning bool if wanted
    /// </summary>
    /// <param name="filter">not needed parameter of filtering list function, return true or false for object</param>
    /// <returns>return list filterd by filter, or full list</returns>
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> list = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        if (filter != null)
        {
            return from item in list
                   where filter(item)
                   select item;
        }
        return from item in list
               select item;
    }
    /// <summary>
    /// Updates Task entity object
    /// </summary>
    /// <param name="item">new Task item - the item with id to update, and values to update</param>
    public void Update(Task item)
    {
        List<Task> list = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        DO.Task? existingItem = (from t in list
                                 where t.Id == item.Id
                                 select t).ToList().FirstOrDefault();

        if (existingItem == null)
        {
            throw new DalDoesNotExistException($"Object of type Task with ID {item.Id} does not exist.");
        }
        else
        {
            list.Remove(existingItem);
            list.Add(item);
        }
        XMLTools.SaveListToXMLSerializer<Task>(list, "tasks");
    }
    public void Deconstruct(DO.Task? t, out int id, out string? discription, out string? alias, out bool? isMilestone, out TimeSpan? duration,
       out DateTime createdAtDate, out DateTime? startDate, out DateTime? scheduledDate,
       out DateTime? deadlineDate, out DateTime? completeDate, out string? deliverables, out string? remarks, out int? engineerId,
       out int? complexityLevel)
    {
        if (t == null)
        {
            id = 0;
            discription = null;
            alias = null;
            isMilestone = null;
            duration = null;
            createdAtDate = DateTime.MinValue;
            startDate = DateTime.MinValue;
            scheduledDate = DateTime.MinValue;
            deadlineDate = DateTime.MinValue;
            completeDate = DateTime.MinValue;
            deliverables = null;
            remarks = null;
            engineerId = null;
            complexityLevel = null;
        }
        else
        {
            id = t.Id;
            discription = t.Discription;
            alias = t.Alias;
            isMilestone = t.IsMilestone;
            duration = t.Duration;
            createdAtDate = t.CreatedAtDate;
            startDate = t.StartDate;
            scheduledDate = t.ScheduledDate;
            deadlineDate = t.DeadlineDate;
            completeDate = t.CompleteDate;
            deliverables = t.Deliverables;
            remarks = t.Remarks;
            engineerId = t.EngineerId;
            complexityLevel = (int)t.ComplexityLevel;
        }
    }
    public void Reset()
    {
        XMLTools.ResetFile("tasks");
    }
}
