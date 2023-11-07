using DO;

namespace Dal;
public class TaskImplementation : DO.ITask
{
    public int Create(DO.Task item)
    {
        int id = DataSource.Config.NextTaskId;
        DO.Task tempItem = new DO.Task(id, item.Discription, item.Alias, item.IsMilestone,
            item.CreatedAtDate, item.StartDate, item.ScheduledDate, item.ForecastDate, item.DeadlineDate,
            item.CompleteDate, item.Deliverables, item.Remarks, item.EngineerId, item.ComplexityLevel);
        DataSource.Tasks.Add(tempItem);
        return id;
    }
    public DO.Task? Read(int id)
    {
        return DataSource.Tasks.Find(Item => Item.Id == id);
    }
    public void Delete(int id)
    {
        DO.Task? deleteIt = DataSource.Tasks.Find(item => item.Id == id);

        if (deleteIt == null)
        {
            throw new Exception($"Object of type T with ID {id} does not exist.");
        }
        else
        {
            DataSource.Tasks.Remove(deleteIt);
        }
    }




    public void Update(DO.Task item)
    {
        DO.Task? existingItem = DataSource.Tasks.Find(Task => Task.Id == item.Id);

        if (existingItem == null)
        {
            throw new Exception($"Object of type T with ID {item.Id} does not exist.");
        }
        else
        {
            DataSource.Tasks.Remove(existingItem);
            DataSource.Tasks.Add(item);
        }
    }

    public List<DO.Task> ReadAll()
    {
        return new List<DO.Task>(DataSource.Tasks);
    }
}