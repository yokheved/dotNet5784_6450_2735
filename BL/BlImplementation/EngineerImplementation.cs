using BlApi;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private readonly DO.IDal _dal = DO.Factory.Get;
    public void AddEngineer(BO.Engineer engineer)
    {
        if (engineer.Id <= 0)
            throw new BO.BlNotValidValueExeption($"value {engineer.Id} for id is not valid");
        if (engineer.Name == "")
            throw new BO.BlNotValidValueExeption($"value {engineer.Name} for id is not valid");
        if (engineer.Cost < 0)
            throw new BO.BlNotValidValueExeption($"value {engineer.Cost} for id is not valid");
        try
        {
            _dal.Engineer!.Create(new DO.Engineer(
                engineer.Id,
                engineer.Name,
                engineer.Email,
                engineer.Level is not null ? (DO.EngineerExperience)engineer.Level : 0,
                engineer.Cost));
        }
        catch (Exception ex)
        {
            if (ex is DO.DalAlreadyExistsException)
                throw new BO.BlAlreadyExistsException(ex.Message);
            else throw new Exception(ex.Message);
        }
    }

    public void DeleteEgineer(int id)
    {
        try
        {
            int? taskId = _dal.Task!.Read(t =>
            t.EngineerId == id && t.StartDate is not null && t.CompleteDate is null
            )?.Id;
            if (taskId is not null)
                throw new BO.BlIsInTheMiddleOfTask($"engineer with id {id} is in the middle of task with id {taskId}");
            _dal.Engineer!.Delete(id);
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException(ex.Message);
            if (ex is BO.BlIsInTheMiddleOfTask)
                throw new BO.BlIsInTheMiddleOfTask(ex.Message);
            else throw new Exception(ex.Message);
        }
    }

    public BO.Engineer GetEngineer(int id)
    {
        string? name = "", email = "";
        int? level = 0;
        double? cost = 0;
        try
        {
            _dal.Engineer!.Deconstruct(
                _dal.Engineer!.Read(id),
                out id,
                out name,
                out email,
                out level,
                out cost);
            return new BO.Engineer()
            {
                Id = id,
                Name = name,
                Email = email,
                Level = level is not null ? (BO.EngineerExperience)level : null,
                Cost = cost ?? 0,
                Task = _dal.Task!.Read(t => t.EngineerId == id && t.StartDate is not null && t.CompleteDate is null) is not null ?
                    (new BO.TaskInEngineer()
                    {
                        Id = _dal.Task!.Read(t => t.EngineerId == id && t.StartDate is not null && t.CompleteDate is null)!.Id,
                        Alias = _dal.Task!.Read(t => t.EngineerId == id && t.StartDate is not null && t.CompleteDate is null)?.Alias
                    })
                    : null
            };
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException(ex.Message);
            else throw new Exception(ex.Message);
        }
    }

    public IEnumerable<BO.Engineer> GetEngineerList(Func<BO.Engineer, bool>? filter)
    {
        try
        {
            if (filter != null)
            {
                Func<DO.Engineer, bool> doFilter =
                    e =>
                    {
                        int level = e.Level is not null ? (int)e.Level : 0;
                        return filter(new BO.Engineer() { Id = e.Id, Name = e.Name, Email = e.Email, Level = (BO.EngineerExperience)level, Cost = e.Cost is not null ? (double)e.Cost : 0 });
                    };
                return from engineer in _dal.Engineer!.ReadAll()
                       where doFilter(engineer)
                       select new BO.Engineer()
                       {
                           Id = engineer.Id,
                           Name = engineer.Name,
                           Email = engineer.Email,
                           Level = engineer.Level is not null ? (BO.EngineerExperience)engineer.Level : 0,
                           Cost = engineer.Cost is not null ? (double)engineer.Cost : 0,
                           Task = _dal.Task!.Read(t => t.EngineerId == engineer.Id && t.StartDate is not null && t.CompleteDate is null) is not null ?
                           (new BO.TaskInEngineer()
                           {
                               Id = _dal.Task!.Read(t => t.EngineerId == engineer.Id && t.StartDate is not null && t.CompleteDate is null)!.Id,
                               Alias = _dal.Task!.Read(t => t.EngineerId == engineer.Id && t.StartDate is not null && t.CompleteDate is null)?.Alias
                           })
                           : null
                       };
            }
            else
                return from engineer in _dal.Engineer!.ReadAll()
                       where true
                       select new BO.Engineer()
                       {
                           Id = engineer.Id,
                           Name = engineer.Name,
                           Email = engineer.Email,
                           Level = engineer.Level is not null ? (BO.EngineerExperience)engineer.Level : 0,
                           Cost = engineer.Cost is not null ? (double)engineer.Cost : 0,
                           Task = _dal.Task!.Read(t => t.EngineerId == engineer.Id && t.StartDate is not null && t.CompleteDate is null) is not null ?
                           (new BO.TaskInEngineer()
                           {
                               Id = _dal.Task!.Read(t => t.EngineerId == engineer.Id && t.StartDate is not null && t.CompleteDate is null)!.Id,
                               Alias = _dal.Task!.Read(t => t.EngineerId == engineer.Id && t.StartDate is not null && t.CompleteDate is null)?.Alias
                           })
                           : null
                       };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public void UpdateEngineer(BO.Engineer engineer)
    {
        if (engineer.Id <= 0)
            throw new BO.BlNotValidValueExeption($"value {engineer.Id} for id is not valid");
        if (engineer.Name == "")
            throw new BO.BlNotValidValueExeption($"value {engineer.Name} for id is not valid");
        if (engineer.Cost < 0)
            throw new BO.BlNotValidValueExeption($"value {engineer.Cost} for id is not valid");
        try
        {
            _dal.Engineer!.Update(new DO.Engineer(
                engineer.Id,
                engineer.Name,
                engineer.Email,
                engineer.Level is not null ? (DO.EngineerExperience)engineer.Level : 0,
                engineer.Cost));
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException(ex.Message);
            else throw new Exception(ex.Message);
        }
    }
}
