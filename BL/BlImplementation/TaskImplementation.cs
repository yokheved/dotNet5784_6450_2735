using BlApi;
namespace BlImplementation;

internal class TaskImplementation : ITask
{
    public void AddTask(BO.Task task)
    {
        throw new NotImplementedException();
    }

    public void DeleteTask(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Task GetTask(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Task> GetTasks(Func<BO.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void UpdateTask(BO.Task task)
    {
        throw new NotImplementedException();
    }
}
