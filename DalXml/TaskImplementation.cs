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
        Task tempItem = new Task(id, item.Discription, item.Alias, item.IsMilestone,
            item.CreatedAtDate, item.StartDate, item.ScheduledDate, item.ForecastDate, item.DeadlineDate,
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
                          select t).ToList()[0];

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
    public Task? Read(int id)
    {
        List<Task> list = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        return (from t in list
                where t.Id == id
                select t).ToList()[0];
    }

    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> list = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        return list
           .FirstOrDefault(filter);
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
                                 select t).ToList()[0];

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
}
