using DO;

namespace Dal;
public class TaskImplementation : Task
{
    public int Create(ITask item)
    {
        int id = DataSource.Config.NextTaskId;
        Dependency tempItem = new Dependency(id, item.DependentTask, item.DependsOnTask);
        DataSource.Tasks.Add(tempItem);
        return id;
    }
    public ITask? Read(int id)
    {
        return DataSource.Tasks.Find(ID => Item.id == ID);
    }
    public void Delete(int id)
    {
        ITask deleteIt = DataSource.Tasks.Find(item => item.id == id);

        if (deleteIt == null)
        {
            throw new Exception($"Object of type T with ID {id} does not exist.");
        }
        else
        {
            DataSource.Tasks.Remove(deleteIt);
        }
    }




    public void Update(ITask item)
    {
        ITask existingItem = DataSource.Tasks.Find(dependency => dependency.Id == item.Id);

        if (existingItem == null)
        {
            throw new Exception($"Object of type T with ID {item.id} does not exist.");
        }
        else
        {
            DataSource.Tasks.Remove(existingItem);
            DataSource.Tasks.Add(item);
        }
    }
   

   
