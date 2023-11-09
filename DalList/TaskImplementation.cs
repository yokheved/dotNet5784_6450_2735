using DO;

namespace Dal;
public class TaskImplementation : ITask
{
    public int Create(DO.Task item)///The "Create" function checks if there is already a task with the same ID in the list of tasks. If there is, it throws an exception. If not, it creates a new task with a new ID.
    {
        if (DataSource.Tasks.Contains(item))
        {
            throw new Exception($"Object of type Task with ID {item.Id} exists.");
        }
        int id = DataSource.Config.NextTaskId;
        DO.Task tempItem = new DO.Task(id, item.Discription, item.Alias, item.IsMilestone,
            item.CreatedAtDate, item.StartDate, item.ScheduledDate, item.ForecastDate, item.DeadlineDate,
            item.CompleteDate, item.Deliverables, item.Remarks, item.EngineerId, item.ComplexityLevel);
        DataSource.Tasks.Add(tempItem);
        return id;
    }
    public DO.Task? Read(int id)/// The "Read" function searches for and returns the task with the given ID from the list of tasks
    {
        return DataSource.Tasks.Find(Item => Item.Id == id);
    }
    public void Delete(int id)/// The "Delete" function searches for the task with the given ID from the list of tasks and removes it. If the task is not found, it throws an exception.
    {
        DO.Task? deleteIt = DataSource.Tasks.Find(item => item.Id == id);

        if (deleteIt == null)
        {
            throw new Exception($"Object of type Task with ID {id} does not exist.");
        }
        else
        {
            DataSource.Tasks.Remove(deleteIt);
        }
    }
    public void Update(DO.Task item)///The "Update" function updates the task with the given ID from the list of tasks
    {
        DO.Task? existingItem = DataSource.Tasks.Find(Task => Task.Id == item.Id);

        if (existingItem == null)
        {
            throw new Exception($"Object of type Task with ID {item.Id} does not exist.");
        }
        else
        {
            DataSource.Tasks.Remove(existingItem);
            DataSource.Tasks.Add(item);
        }
    }

    public List<DO.Task> ReadAll()///The "ReadAll" function returns all tasks from the list of tasks as a new list
    {
        return new List<DO.Task>(DataSource.Tasks);
    }
}