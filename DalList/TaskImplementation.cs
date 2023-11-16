﻿using DO;

namespace Dal;
internal class TaskImplementation : ITask
{
    //function declarations and comants in ITask
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
                select t).ToList()[0];
    }
    /// <summary>
    /// Deletes a Task object by its Id
    /// </summary>
    /// <param name="id">id of Task to delete</param>
    public void Delete(int id)
    {
        DO.Task? deleteIt = (from t in DataSource.Tasks
                             where t.Id == id
                             select t).ToList()[0];

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
                                 select t).ToList()[0];

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
    /// Reads all Task entity objects
    /// </summary>
    /// <returns> list type Task of all Tasks</returns>
    public List<DO.Task> ReadAll()
    {
        return (from t in DataSource.Tasks
                where true
                select t).ToList();
    }
}