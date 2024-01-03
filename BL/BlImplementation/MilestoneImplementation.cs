using BlApi;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private readonly DO.IDal _dal = DO.Factory.Get;

    private List<BO.Milestone> CreateMilestonesList(List<BO.Task> tasksList)
    {
        var dependencies = _dal.Dependency!.ReadAll().ToList();
        var dict = (
           from d in dependencies
           where tasksList.Any(t => t.Id == d.DependentTask) && tasksList.Any(t => t.Id == d.DependsOnTask)
           group d by d.DependentTask into newList
           let depList = (from dep in newList
                          select dep.DependsOnTask)
           select new { Key = newList.Key, Value = depList.Order() }).ToList();
        var distinctDict = dict.Count == 1 ? dict : dict.Distinct();
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
        int lastKey = distinctDict?.First().Key - 1 ?? -1;
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
            List<int> deptList = new List<int>();
            //create dependency: tasks that are dependent on current milestone
            foreach (var task in item.Value)
            {
                deptList.Add(task);
                _dal.Dependency!.Create(new DO.Dependency(0, id, task));
            }
            //create dependency: milestone dependent on tasks
            foreach (var task in dict)
            {
                if (task.Value.SequenceEqual(deptList))
                    _dal.Dependency!.Create(new DO.Dependency(0, task.Key, id));
            }
            mileStoneList.Add(this.GetMilestone(id));
        }
        foreach (var k in tasksList)//for each task that does not have dependencies - dependent on start
        {
            try
            {
                _dal.Dependency!.Read(de => de.DependentTask == k.Id);
            }
            catch { _dal.Dependency!.Create(new DO.Dependency(0, k.Id, idStart)); }
        };
        mileStoneList.Insert(0, this.GetMilestone(idStart));//add start milestone to the beggining of milestones 
        int idEnd = _dal.Task!.Create(new DO.Task(//create start milestone
                0,
                null,
                $"M {i} : END",
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
        (from t in tasksList
         where (from milestone in mileStoneList
                where milestone.DependenciesList!.Any(d => d.Id == t.Id)
                select milestone.Id).ToList().Count() == 0
         select t).ToList().ForEach(d =>//create dependencies for END milstone
         {
             try { _dal.Dependency!.Read(de => de.DependsOnTask == d.Id); }
             catch { _dal.Dependency!.Create(new DO.Dependency(0, idEnd, d.Id)); }
         });
        mileStoneList.Add(this.GetMilestone(idEnd));//add end milestone to the end of milestones 
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
                0));
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
            List<BO.Milestone> milestones = CreateMilestonesList(tasksList.ToList());
            for (int i = 1; i < milestones.Count; i++)//ApproxStartAtDate calculation for all milestones
            {
                milestones[i] = GetMilestone(milestones[i].Id);
            }
            List<BO.Task> tasks = Factory.Get.Task.GetTasks()
                .Where(t => tasksList.Any(ta => ta.Id == t.Id)).Select(t => t).ToList();
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
                    milestones[i + 1].ApproxStartAtDate;
            }
            tasks.ForEach(t =>//update new data in DAL
            {
                t.ApproxStartAtDate = GetMilestone(t.Milestone!.Id).ApproxStartAtDate;
                t.LastDateToEnd = (from m in milestones where m.DependenciesList!.Any(d => t.Id == d.Id) select m).First().ApproxStartAtDate;
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
                t.DependenciesList?.ForEach(d =>
                {
                    t.Alias += $"{d.Alias}";
                });
                _dal.Task!.Update(new DO.Task(//also checks for circling dependencies
                    t.Id,
                    null,
                    t.Alias,
                    true,
                    null,
                    t.CreateAtDate,
                    t.StartAtDate,
                    t.ApproxStartAtDate,
                    t.LastDateToEnd,
                    t.EndAtDate,
                    null,
                    t.Remarks,
                    null,
                    0));
            });
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException || ex is BO.BlDoesNotExistException)
                throw new BO.BlDoesNotExistException(ex.Message, ex);
            if (ex is DO.DalAlreadyExistsException) throw new BO.BlAlreadyExistsException(ex.Message, ex);
            if (ex is BO.BlCirclingDependenciesExeption) throw new BO.BlCirclingDependenciesExeption(ex.Message, ex);
            throw new Exception(ex.Message, ex);
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
        {//פה יש שגיאת ריצה, לבדוק
            _dal.Task!.Deconstruct(//WORKES
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
                DependenciesList = _dal.Dependency!.ReadAll(d => d.DependentTask == id).Select(d =>
                new BO.TaskInList()
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
                throw new BO.BlDoesNotExistException(ex.Message, ex);
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
