using BlApi;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private readonly DO.IDal _dal = DO.Factory.Get;

    private List<BO.Milestone> CreateMilestonesList(List<BO.Task> tasksList)
    {
        var dict = _dal.Dependency!.ReadAll(d => tasksList.Any(t => t.Id == d.DependentTask) && tasksList.Any(t => t.Id == d.DependsOnTask))
            .GroupBy(d => d.DependentTask).ToDictionary(
                group => group.Key,//dependent task
                group => group.Select(d => d.DependentTask)//all dependencies for task
            ).OrderBy(pair => pair.Key)
                             .ToDictionary(pair => pair.Key, pair => pair.Value);
        var distinctDict = dict.Distinct();
        _dal.Dependency!.Reset();
        int idStart = _dal.Task!.Create(new DO.Task(//create start milestone
                0,
                null,
                "M 0 : START",
                true,
                null,
                DateTime.Now,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                0));
        int i = 1;
        List<BO.Milestone> mileStoneList = new List<BO.Milestone>();
        List<int> keys = new List<int>();
        foreach (var item in distinctDict)//add all milestones
        {
            int id = _dal.Task!.Create(new DO.Task(
                0,
                null,
                $"M {i++}",
                true,
                null,
                DateTime.Now,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                0));
            item.Value.ToList().ForEach(d =>//create dependencies for this milstone
            {
                if (_dal.Dependency!.Read(de => de.DependentTask == id && de.DependsOnTask == d) is null)
                    _dal.Dependency!.Create(new DO.Dependency(0, id, d));
            });
            mileStoneList.Add(this.GetMilestone(id));
            //get all dependencies from dict that are supposed to depened on current milestone
            keys = keys.Concat(from d in dict
                               where d.Key == item.Key
                               select d.Key).ToList();
            keys.ForEach(k =>//for all milestones dependent on this milestone creat dependency
             {
                 if (_dal.Dependency!.Read(de => de.DependentTask == k && de.DependsOnTask == id) is null)
                 {
                     _dal.Dependency!.Create(new DO.Dependency(0, k, id));
                     dict.Remove(k);
                 }
             });
        }
        foreach (var k in dict)//for each task that does not have dependencies - dependent on start
        {
            if (_dal.Dependency!.Read(de => de.DependentTask == k.Key && de.DependsOnTask == idStart) is null)
                _dal.Dependency!.Create(new DO.Dependency(0, k.Key, idStart));
            dict.Remove(k.Key);
        };
        mileStoneList.Insert(0, this.GetMilestone(idStart));//add start milestone to the beggining of milestones 
        foreach (var task in tasksList)//update dependency and milestone property for all tasks
        {
            task.DependenciesList = (from d in _dal.Dependency!.ReadAll(d => d.DependentTask == task.Id)
                                     where true
                                     select new BO.TaskInList()
                                     {
                                         Id = d.DependsOnTask,
                                         Alias = _dal.Task!.Read(d.DependsOnTask)?.Alias,
                                         Description = _dal.Task!.Read(d.DependsOnTask)?.Discription,
                                         Status = (BO.Status)(_dal.Task!.Read(d.DependsOnTask)?.ScheduledDate is null ? 0
                                             : _dal.Task!.Read(d.DependsOnTask)?.StartDate is null ? 1
                                             : _dal.Task!.Read(d.DependsOnTask)?.CompleteDate is null ? 2
                                             : 3)
                                     }).ToList();
            task.Milestone = GetMilstoneForProjectCreate(task.Id);
        }
        return mileStoneList;
    }
    private BO.MilestoneInTask? GetMilstoneForProjectCreate(int taskId)
    {
        try
        {
            return new BO.MilestoneInTask()
            {
                Id = _dal.Task!.Read(_dal.Dependency!.Read(d =>
                    d.DependsOnTask == taskId && _dal.Task!.Read(ta => ta.IsMilestone && ta.Id == d.DependentTask) is not null
                                    )!.DependentTask)!.Id,
                Alias = _dal.Task!.Read(_dal.Dependency!.Read(d =>
                    d.DependsOnTask == taskId && (_dal.Task!.Read(ta => ta.IsMilestone && ta.Id == d.DependentTask) is not null)
                                    )!.DependentTask)?.Alias
            };
        }
        catch { return null; }
    }
    public void UpdateMilestone(int id)
    {
        try
        {
            BO.Milestone milestone = GetMilestone(id);
            _dal.Task!.Update(new DO.Task(
                milestone.Id,
                milestone.Description,
                milestone.Alias,
                true,
                null,
                milestone.CreateAtDate,
                milestone.DependenciesList!.Select(d => _dal.Task!.Read(d.Id)!.StartDate).First() ??
                milestone.StartAtDate,
                milestone.ApproxStartAtDate,
                milestone.LastDateToEnd,
                milestone.DependenciesList!.All(d => _dal.Task!.Read(d.Id)!.CompleteDate is not null) ? DateTime.Now :
                milestone.EndAtDate,
                null,
                milestone.Remarks,
                null,
                (DO.EngineerExperience)0));
            _dal.Dependency!.ReadAll(d => d.DependentTask == milestone.Id && milestone.DependenciesList!.Any(de => d.DependsOnTask != de.Id))
                .ToList().ForEach(d => _dal.Dependency!.Delete(d.Id));
            milestone.DependenciesList!.ForEach(d =>
            {
                try { _dal.Dependency!.Read(de => de.DependentTask == milestone.Id && de.DependsOnTask == d.Id); }
                catch { _dal.Dependency!.Create(new DO.Dependency(0, milestone.Id, d.Id)); }
            });
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException(ex.Message, ex);
            if (ex is BO.BlCirclingDependenciesExeption)
                throw new BO.BlCirclingDependenciesExeption(ex.Message, ex);
            else throw new Exception(ex.Message, ex);
        }
    }
    public void CreateProjectSchedule(DateTime startDate, DateTime endDate, IEnumerable<BO.Task> tasksList)
    {
        try
        {
            List<BO.Task> tasks = tasksList.ToList();
            List<BO.Milestone> milestones = this.CreateMilestonesList(tasks);
            milestones.First().ApproxStartAtDate = startDate;
            milestones.Last().LastDateToEnd = endDate;
            for (int i = 1; i < milestones.Count; i++)//ApproxStartAtDate calculation for all milestones
            {
                milestones[i].ApproxStartAtDate =
                    milestones[i - 1].ApproxStartAtDate + tasks.Max(t => t.Duration) ?? DateTime.Now;
            }
            for (int i = milestones.Count - 2; i == 0; i++)//LastDateToEnd calculation for all milestones
            {
                milestones[i].LastDateToEnd =
                    milestones[i - 1].LastDateToEnd - tasks.Max(t => t.Duration) ?? DateTime.Now;
            }
            tasks.ForEach(t =>//update new data in DAL
            {
                t.ApproxStartAtDate = GetMilestone(t.Milestone!.Id).ApproxStartAtDate;
                t.LastDateToEnd = (from m in milestones where m.DependenciesList!.Any(d => d.Id == m.Id) select m).First().ApproxStartAtDate;
                _dal.Task!.Update(new DO.Task(
                    t.Id,
                    t.Description,
                    t.Alias,
                    false,
                    t.Duration,
                    t.CreatedAtDate,
                    t.StartAtDate,
                    t.ApproxStartAtDate,
                    t.LastDateToEnd,
                    t.EndAtDate,
                    t.Deliverables,
                    t.Remarks,
                    t.Engineer?.Id,
                    t.Level is not null ? (DO.EngineerExperience)t.Level : 0));
            });
            milestones.ForEach(t =>//update new data in DAL
            {
                t.Alias = "";
                t.DependenciesList?.ForEach(d =>
                {
                    t.Alias += $"{d.Alias}";
                });
                _dal.Task!.Update(new DO.Task(//also checks for circling dependencies
                    t.Id,
                    null,
                    t.Alias,
                    false,
                    null,
                    t.CreateAtDate,
                    t.StartAtDate,
                    t.ApproxStartAtDate,
                    t.LastDateToEnd,
                    t.EndAtDate,
                    null,
                    t.Remarks,
                    null,
                    (DO.EngineerExperience)0));
            });
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException || ex is BO.BlDoesNotExistException)
                throw new BO.BlDoesNotExistException(ex.Message, ex);
            if (ex is DO.DalAlreadyExistsException) throw new BO.BlAlreadyExistsException(ex.Message, ex);
            if (ex is BO.BlCirclingDependenciesExeption) throw new BO.BlCirclingDependenciesExeption(ex.Message, ex);
            throw new Exception(ex.Message);
        }
    }
    public BO.Milestone GetMilestone(int id)
    {
        string? description = "", alias = "", remarks = "", delivarables = "";
        int? level = 0, engineerId = 0;
        bool? isMilestone = false;
        TimeSpan? duration = null;
        double complex = 0;
        DateTime created = DateTime.MinValue;
        DateTime? start = null, scheduled = null, deadline = null, completed = null;
        try
        {
            _dal.Task!.Deconstruct(
                _dal.Task!.Read(t => t.Id == id && t.IsMilestone),
                out id,
                out description,
                out alias,
                out isMilestone,
                out duration,
                out created,
                out start,
                out scheduled,
                out deadline,
                out completed,
                out delivarables,
                out remarks,
                out engineerId,
                out level);
            (from d in _dal.Dependency!.ReadAll(d => d.DependentTask == id)
             where true
             select new BO.TaskInList()
             {
                 Id = d.DependsOnTask,
                 Alias = _dal.Task!.Read(d.DependsOnTask)?.Alias,
                 Description = _dal.Task!.Read(d.DependsOnTask)?.Discription,
                 Status = (BO.Status)(_dal.Task!.Read(d.DependsOnTask)?.ScheduledDate is null ? 0
                     : _dal.Task!.Read(d.DependsOnTask)?.StartDate is null ? 1
                     : _dal.Task!.Read(d.DependsOnTask)?.CompleteDate is null ? 2
                     : 3)
             }).ToList().ForEach(tl => complex += ((int)tl.Status + 1) * 25.0 / _dal.Dependency!.ReadAll(d => d.DependentTask == id).Count());
            return new BO.Milestone()
            {
                Status = (BO.Status)(scheduled is null ? 0
                               : start is null ? 1
                               : completed is null ? 2
                               : 3),
                Id = id,
                Alias = alias,
                Description = description,
                CreateAtDate = created,
                StartAtDate = start ?? DateTime.MinValue,
                Remarks = remarks,
                ApproxStartAtDate = scheduled ?? DateTime.MinValue,
                LastDateToEnd = deadline ?? DateTime.MinValue,
                EndAtDate = completed,
                DependenciesList = (from d in _dal.Dependency!.ReadAll(d => d.DependentTask == id)
                                    where true
                                    select new BO.TaskInList()
                                    {
                                        Id = d.DependsOnTask,
                                        Alias = _dal.Task!.Read(d.DependsOnTask)?.Alias,
                                        Description = _dal.Task!.Read(d.DependsOnTask)?.Discription,
                                        Status = (BO.Status)(_dal.Task!.Read(d.DependsOnTask)?.ScheduledDate is null ? 0
                                            : _dal.Task!.Read(d.DependsOnTask)?.StartDate is null ? 1
                                            : _dal.Task!.Read(d.DependsOnTask)?.CompleteDate is null ? 2
                                            : 3)
                                    }).ToList(),
                CompletionPercentage = complex
            };
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException(ex.Message);
            else throw new Exception(ex.Message);
        }
    }
    public BO.Milestone UpdateMilestone(int id, string? alias, string? description, string? remarks)
    {
        if (id <= 0)
            throw new BO.BlNotValidValueExeption($"value {id} for id is not valid");
        if (alias == "")
            throw new BO.BlNotValidValueExeption($"value{alias} for alias is not valid");
        try
        {
            BO.Milestone milestone = GetMilestone(id);
            milestone.Description = description ?? milestone.Description;
            milestone.Remarks = remarks ?? milestone.Remarks;
            milestone.Alias = alias ?? milestone.Alias;
            _dal.Task!.Update(new DO.Task(
                milestone.Id,
                description ?? milestone.Description,
                alias ?? milestone.Alias,
                true,
                null,
                milestone.CreateAtDate,
                milestone.StartAtDate,
                milestone.ApproxStartAtDate,
                milestone.LastDateToEnd,
                milestone.EndAtDate,
                null,
                remarks ?? milestone.Remarks,
                null,
                (DO.EngineerExperience)0));
            return milestone;
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException(ex.Message);
            if (ex is BO.BlCirclingDependenciesExeption)
                throw new BO.BlCirclingDependenciesExeption(ex.Message);
            else throw new Exception(ex.Message);
        }
    }
}
