using BlApi;
namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private readonly DO.IDal _dal = DO.Factory.Get;
    public void AddTask(BO.Task task)
    {
        if (task.Id <= 0)
            throw new BO.BlNotValidValueExeption($"value {task.Id} for id is not valid");
        if (task.Alias == "")
            throw new BO.BlNotValidValueExeption($"value{task.Alias} for alias is not valid");
        try
        {
            _dal.Task!.Create(new DO.Task(
                task.Id,
                task.Description,
                task.Alias,
                task.Milestone is not null,
                task.CreatedAtDate,
                task.StartAtDate,
                task.ApproxStartAtDate,
                task.ApproxEndAtDate,
                task.LastDateToEnd,
                null,
                task.Deliverables,
                task.Remarks,
                task.Engineer?.Id,
                task.Level is not null ? (DO.EngineerExperience)task.Level : null));
        }
        catch (Exception ex)
        {
            if (ex is DO.DalAlreadyExistsException)
                throw new BO.BlAlreadyExistsException(ex.Message);
            else throw new Exception(ex.Message);
        }
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
