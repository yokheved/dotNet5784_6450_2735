using DO;

namespace Dal;
internal class TaskImplementation : ITask
{

    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        return DataSource.Tasks
            .FirstOrDefault(filter);
    }
    /// <summary>
    /// Creates new Task entity object in DAL
    /// </summary>
    /// <param name="item">Task type</param>
    /// <returns>integer - new Task item id</returns>
    public int Create(DO.Task item)
    {
        if (DataSource.Tasks.Any(t => t == item))
        {
            throw new DalAlreadyExistsException($"Object of type Task with ID {item.Id} exists.");
        }
        int id = DataSource.Config.NextTaskId;
        DO.Task tempItem = new DO.Task(id, item.Discription, item.Alias, item.IsMilestone,
            item.CreatedAtDate, item.StartDate, item.ScheduledDate, item.ForecastDate, item.DeadlineDate,
            item.CompleteDate, item.Deliverables, item.Remarks, item.EngineerId, item.ComplexityLevel);
        DataSource.Tasks.Add(tempItem);
        return id;
    }
    /// <summary>
    /// Reads Task entity object by its ID 
    /// </summary>
    /// <param name="id">id of Task to read</param>
    /// <returns>Task object with param id, if not found - null</returns>
    public DO.Task? Read(int id)
    {
        return (from t in DataSource.Tasks
                where t.Id == id
                select t).ToList().FirstOrDefault();
    }
    /// <summary>
    /// Deletes a Task object by its Id
    /// </summary>
    /// <param name="id">id of Task to delete</param>
    public void Delete(int id)
    {
        DO.Task? deleteIt = (from t in DataSource.Tasks
                             where t.Id == id
                             select t).ToList().FirstOrDefault();

        if (deleteIt == null)
        {
            throw new DalAlreadyExistsException($"Object of type Task with ID {id} does not exist.");
        }
        else
        {
            DataSource.Tasks.Remove(deleteIt);
        }
    }
    /// <summary>
    /// Updates Task entity object
    /// </summary>
    /// <param name="item">new Task item - the item with id to update, and values to update</param>
    public void Update(DO.Task item)
    {
        DO.Task? existingItem = (from t in DataSource.Tasks
                                 where t.Id == item.Id
                                 select t).ToList().FirstOrDefault();

        if (existingItem == null)
        {
            throw new DalDoesNotExistException($"Object of type Task with ID {item.Id} does not exist.");
        }
        else
        {
            DataSource.Tasks.Remove(existingItem);
            DataSource.Tasks.Add(item);
        }
    }
    /// <summary>
    /// Reads all entity objects by lambda function returning bool if wanted
    /// </summary>
    /// <param name="filter">not needed parameter of filtering list function, return true or false for object</param>
    /// <returns>return list filterd by filter, or full list</returns>
    public IEnumerable<DO.Task> ReadAll(Func<DO.Task, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item;
    }

    public void Deconstruct(DO.Task? t, out int id, out string? discription, out string? alias, out bool? isMilestone,
        out DateTime createdAtDate, out DateTime? startDate, out DateTime? scheduledDate, out DateTime? forecastDate,
        out DateTime? deadlineDate, out DateTime? completeDate, out string? deliverables, out string? remarks, out int? engineerId,
        out int? complexityLevel)
    {
        if (t == null)
        {
            id = 0;
            discription = null;
            alias = null;
            isMilestone = null;
            createdAtDate = DateTime.MinValue;
            startDate = DateTime.MinValue;
            scheduledDate = DateTime.MinValue;
            deadlineDate = DateTime.MinValue;
            completeDate = DateTime.MinValue;
            forecastDate = DateTime.MinValue;
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
            createdAtDate = t.CreatedAtDate;
            startDate = t.StartDate;
            scheduledDate = t.ScheduledDate;
            deadlineDate = t.DeadlineDate;
            completeDate = t.CompleteDate;
            deliverables = t.Deliverables;
            forecastDate = t.ForecastDate;
            remarks = t.Remarks;
            engineerId = t.EngineerId;
            complexityLevel = t.ComplexityLevel;
        }
    }
}