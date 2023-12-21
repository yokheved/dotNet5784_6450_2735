using BlApi;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private readonly DO.IDal _dal = DO.Factory.Get;
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
                _dal.Task!.Read(id),
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
                CompletionPercentage = complex,
            };
        }
        catch (Exception ex)
        {
            if (ex is DO.DalDoesNotExistException)
                throw new BO.BlDoesNotExistException(ex.Message);
            else throw new Exception(ex.Message);
        }
    }

    public BO.Milestone UpdateMilestone(int id)
    {
        //not clear how to update relevent fields
        throw new NotImplementedException();
    }
}
