using BlApi;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private readonly DO.IDal _dal = DO.Factory.Get;
    public int AddTask(BO.Task task)
    {
        if (task.Id <= 0)
            throw new BO.BlNotValidValueExeption($"value {task.Id} for id is not valid");
        if (task.Alias == "")
            throw new BO.BlNotValidValueExeption($"value{task.Alias} for alias is not valid");
        try
        {
            return _dal.Task!.Create(new DO.Task(
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
        try
        {
            int? taskId = _dal.Task!.Read(t =>
            t.EngineerId == id && t.StartDate is not null && t.CompleteDate is null
            )?.Id;
            int? dependentTask = _dal.Dependency!.Read(d =>
            d.DependsOnTask == taskId)?.DependentTask;
            if (dependentTask is not null)
                throw new BO.BlIsADependencyExeption($"task with id {id} is  a dependency for task {dependentTask}");
            _dal.Engineer!.Delete(id);
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException(ex.Message);
            if (ex is BO.BlIsADependencyExeption)
                throw new BO.BlIsADependencyExeption(ex.Message);
            else throw new Exception(ex.Message);
        }
    }

    public BO.Task GetTask(int id)
    {
        string? description = "", alias = "", remarks = "", delivarables = "";
        int? level = 0, engineerId = 0;
        bool? isMilestone = false;
        DateTime created = DateTime.MinValue;
        DateTime? start = null, scheduled = null, forcast = null, deadline = null, completed = null;
        try
        {
            _dal.Task!.Deconstruct(
                _dal.Task!.Read(id),
                out id,
                out description,
                out alias,
                out isMilestone,
                out created,
                out start,
                out scheduled,
                out forcast,
                out deadline,
                out completed,
                out delivarables,
                out remarks,
                out engineerId,
                out level);
            return new BO.Task()
            {
                Id = id,
                Description = description,
                Alias = alias,
                Level = level is not null ? (BO.EngineerExperience)level : null,
                StartAtDate = start ?? DateTime.MinValue,
                Remarks = remarks,
                Deliverables = delivarables,
                ApproxEndAtDate = forcast ?? DateTime.MinValue,
                ApproxStartAtDate = scheduled ?? DateTime.MinValue,
                LastDateToEnd = deadline ?? DateTime.MinValue,
                Engineer = new BO.EngineerInTask()
                {
                    Id = _dal.Engineer!.Read(t => t.Id == engineerId)!.Id,
                    Name = _dal.Engineer!.Read(t => t.Id == engineerId)!.Name
                }
            };
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException(ex.Message);
            else throw new Exception(ex.Message);
        }
    }

    public IEnumerable<BO.Task> GetTasks(Func<BO.Task, bool>? filter = null)
    {
        try
        {
            if (filter != null)
            {
                Func<DO.Task, bool> doFilter =
                    t =>
                    {
                        int level = t.ComplexityLevel is not null ? (int)t.ComplexityLevel : 0;
                        return filter(new BO.Task()
                        {
                            Id = t.Id,
                            Description = t.Discription,
                            Alias = t.Alias,
                            Level = (BO.EngineerExperience)level,
                            StartAtDate = t.StartDate ?? DateTime.MinValue,
                            Remarks = t.Remarks,
                            Deliverables = t.Deliverables,
                            ApproxEndAtDate = t.ForecastDate ?? DateTime.MinValue,
                            ApproxStartAtDate = t.ScheduledDate ?? DateTime.MinValue,
                            LastDateToEnd = t.DeadlineDate ?? DateTime.MinValue,
                            Engineer = new BO.EngineerInTask()
                            {
                                Id = _dal.Engineer!.Read(e => e.Id == t.EngineerId)!.Id,
                                Name = _dal.Engineer!.Read(e => e.Id == t.EngineerId)!.Name
                            }
                        });
                    };
                return from t in _dal.Task!.ReadAll()
                       where doFilter(t)
                       select new BO.Task()
                       {
                           Id = t.Id,
                           Description = t.Discription,
                           Alias = t.Alias,
                           Level = t.ComplexityLevel is not null ? (BO.EngineerExperience)t.ComplexityLevel : 0,
                           StartAtDate = t.StartDate ?? DateTime.MinValue,
                           Remarks = t.Remarks,
                           Deliverables = t.Deliverables,
                           ApproxEndAtDate = t.ForecastDate ?? DateTime.MinValue,
                           ApproxStartAtDate = t.ScheduledDate ?? DateTime.MinValue,
                           LastDateToEnd = t.DeadlineDate ?? DateTime.MinValue,
                           Engineer = new BO.EngineerInTask()
                           {
                               Id = _dal.Engineer!.Read(e => e.Id == t.EngineerId)!.Id,
                               Name = _dal.Engineer!.Read(e => e.Id == t.EngineerId)!.Name
                           }
                       };
            }
            else
                return from t in _dal.Task!.ReadAll()
                       where true
                       select new BO.Task()
                       {
                           Id = t.Id,
                           Description = t.Discription,
                           Alias = t.Alias,
                           Level = t.ComplexityLevel is not null ? (BO.EngineerExperience)t.ComplexityLevel : 0,
                           StartAtDate = t.StartDate ?? DateTime.MinValue,
                           Remarks = t.Remarks,
                           Deliverables = t.Deliverables,
                           ApproxEndAtDate = t.ForecastDate ?? DateTime.MinValue,
                           ApproxStartAtDate = t.ScheduledDate ?? DateTime.MinValue,
                           LastDateToEnd = t.DeadlineDate ?? DateTime.MinValue,
                           Engineer = new BO.EngineerInTask()
                           {
                               Id = _dal.Engineer!.Read(e => e.Id == t.EngineerId)!.Id,
                               Name = _dal.Engineer!.Read(e => e.Id == t.EngineerId)!.Name
                           }
                       };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public void UpdateTask(BO.Task task)
    {
        if (task.Id <= 0)
            throw new BO.BlNotValidValueExeption($"value {task.Id} for id is not valid");
        if (task.Alias == "")
            throw new BO.BlNotValidValueExeption($"value{task.Alias} for alias is not valid");
        try
        {
            _dal.Task!.Update(new DO.Task(
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
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException(ex.Message);
            else throw new Exception(ex.Message);
        }
    }
}
