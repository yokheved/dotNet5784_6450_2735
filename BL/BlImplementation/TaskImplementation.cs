using BlApi;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private readonly DO.IDal _dal = DO.Factory.Get;
    private readonly IBl _bl = Factory.Get;
    public int AddTask(BO.Task task)
    {
        if (task.Id <= 0)
            throw new BO.BlNotValidValueExeption($"value {task.Id} for id is not valid");
        if (task.Alias == "")
            throw new BO.BlNotValidValueExeption($"value{task.Alias} for alias is not valid");
        try
        {
            int id = _dal.Task!.Create(new DO.Task(
                task.Id,
                task.Description,
                task.Alias,
                false,
                task.Duration,
                task.CreatedAtDate,
                task.StartAtDate,
                task.ApproxStartAtDate,
                task.LastDateToEnd,
                task.EndAtDate,
                task.Deliverables,
                task.Remarks,
                task.Engineer?.Id,
                task.Level is not null ? (DO.EngineerExperience)task.Level : 0));
            task.DependenciesList!.ForEach(d =>
            {
                if (_dal.Dependency!.Read(de => de.DependentTask == id && de.DependsOnTask == d.Id) is null)
                    _dal.Dependency!.Create(new DO.Dependency(0, id, d.Id));
            });
            _bl.Milestone!.UpdateMilestone(_dal.Task!.Read(_dal.Dependency!.Read(d =>
            {
                _dal.Task!.Read(ta => ta.IsMilestone && ta.Id == d.DependentTask);
                return d.DependsOnTask == task.Id;
            })!.DependentTask)!.Id);
            return id;
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
            _bl.Milestone!.UpdateMilestone(_dal.Task!.Read(_dal.Dependency!.Read(d =>
            {
                _dal.Task!.Read(ta => ta.IsMilestone && ta.Id == d.DependentTask);
                return d.DependsOnTask == taskId;
            })!.DependentTask)!.Id);
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
        TimeSpan? duration = null;
        bool? isMilestone = false;
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
            return new BO.Task()
            {
                Id = id,
                Description = description,
                Alias = alias,
                Duration = duration,
                Level = level is not null ? (BO.EngineerExperience)level : null,
                StartAtDate = start ?? DateTime.MinValue,
                Remarks = remarks,
                Deliverables = delivarables,
                EndAtDate = completed ?? DateTime.MinValue,
                ApproxStartAtDate = scheduled ?? DateTime.MinValue,
                LastDateToEnd = deadline ?? DateTime.MinValue,
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
                Engineer = new BO.EngineerInTask()
                {
                    Id = _dal.Engineer!.Read(t => t.Id == engineerId)!.Id,
                    Name = _dal.Engineer!.Read(t => t.Id == engineerId)!.Name
                },
                Status = (BO.Status)(scheduled is null ? 0
                               : start is null ? 1
                               : completed is null ? 2
                               : 3),
                Milestone = new BO.MilestoneInTask()
                {
                    Id = _dal.Task!.Read(_dal.Dependency!.Read(d =>
                        d.DependsOnTask == id && _dal.Task!.Read(ta => ta.IsMilestone && ta.Id == d.DependentTask) is not null
                                )!.DependentTask)!.Id,
                    Alias = _dal.Task!.Read(_dal.Dependency!.Read(d =>
                        d.DependsOnTask == id && (_dal.Task!.Read(ta => ta.IsMilestone && ta.Id == d.DependentTask) is not null)
                                )!.DependentTask)?.Alias
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
                        return filter(new BO.Task()
                        {
                            Id = t.Id,
                            Description = t.Discription,
                            Alias = t.Alias,
                            Duration = t.Duration,
                            Level = (BO.EngineerExperience)t.ComplexityLevel,
                            StartAtDate = t.StartDate ?? DateTime.MinValue,
                            Remarks = t.Remarks,
                            Deliverables = t.Deliverables,
                            EndAtDate = t.CompleteDate ?? DateTime.MinValue,
                            ApproxStartAtDate = t.ScheduledDate ?? DateTime.MinValue,
                            LastDateToEnd = t.DeadlineDate ?? DateTime.MinValue,
                            Engineer = new BO.EngineerInTask()
                            {
                                Id = _dal.Engineer!.Read(e => e.Id == t.EngineerId)!.Id,
                                Name = _dal.Engineer!.Read(e => e.Id == t.EngineerId)!.Name
                            },
                            DependenciesList = (from d in _dal.Dependency!.ReadAll(d => d.DependentTask == t.Id)
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
                            Status = (BO.Status)(t.ScheduledDate is null ? 0
                               : t.StartDate is null ? 1
                               : t.CompleteDate is null ? 2
                               : 3),
                            Milestone = new BO.MilestoneInTask()
                            {
                                Id = _dal.Task!.Read(_dal.Dependency!.Read(d =>
                                {
                                    _dal.Task!.Read(ta => ta.IsMilestone && ta.Id == d.DependentTask);
                                    return d.DependsOnTask == t.Id;
                                })!.DependentTask)!.Id,
                                Alias = _dal.Task!.Read(_dal.Dependency!.Read(d =>
                                {
                                    _dal.Task!.Read(ta => ta.IsMilestone && ta.Id == d.DependentTask);
                                    return d.DependsOnTask == t.Id;
                                })!.DependentTask)?.Alias
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
                           Duration = t.Duration,
                           Level = (BO.EngineerExperience)t.ComplexityLevel,
                           StartAtDate = t.StartDate ?? DateTime.MinValue,
                           Remarks = t.Remarks,
                           Deliverables = t.Deliverables,
                           EndAtDate = t.CompleteDate ?? DateTime.MinValue,
                           ApproxStartAtDate = t.ScheduledDate ?? DateTime.MinValue,
                           LastDateToEnd = t.DeadlineDate ?? DateTime.MinValue,
                           Engineer = new BO.EngineerInTask()
                           {
                               Id = _dal.Engineer!.Read(e => e.Id == t.EngineerId)!.Id,
                               Name = _dal.Engineer!.Read(e => e.Id == t.EngineerId)!.Name
                           },
                           DependenciesList = (from d in _dal.Dependency!.ReadAll(d => d.DependentTask == t.Id)
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
                           Status = (BO.Status)(t.ScheduledDate is null ? 0
                               : t.StartDate is null ? 1
                               : t.CompleteDate is null ? 2
                               : 3),
                           Milestone = new BO.MilestoneInTask()
                           {
                               Id = _dal.Task!.Read(_dal.Dependency!.Read(d =>
                               {
                                   _dal.Task!.Read(ta => ta.IsMilestone && ta.Id == d.DependentTask);
                                   return d.DependsOnTask == t.Id;
                               })!.DependentTask)!.Id,
                               Alias = _dal.Task!.Read(_dal.Dependency!.Read(d =>
                               {
                                   _dal.Task!.Read(ta => ta.IsMilestone && ta.Id == d.DependentTask);
                                   return d.DependsOnTask == t.Id;
                               })!.DependentTask)?.Alias
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
                           Duration = t.Duration,
                           Alias = t.Alias,
                           Level = (BO.EngineerExperience)t.ComplexityLevel,
                           StartAtDate = t.StartDate ?? DateTime.MinValue,
                           Remarks = t.Remarks,
                           Deliverables = t.Deliverables,
                           ApproxStartAtDate = t.ScheduledDate ?? DateTime.MinValue,
                           LastDateToEnd = t.DeadlineDate ?? DateTime.MinValue,
                           DependenciesList = (from d in _dal.Dependency!.ReadAll(d => d.DependentTask == t.Id)
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
                           Engineer = new BO.EngineerInTask()
                           {
                               Id = _dal.Engineer!.Read(e => e.Id == t.EngineerId)!.Id,
                               Name = _dal.Engineer!.Read(e => e.Id == t.EngineerId)!.Name
                           },
                           Status = (BO.Status)(t.ScheduledDate is null ? 0
                               : t.StartDate is null ? 1
                               : t.CompleteDate is null ? 2
                               : 3),
                           Milestone = new BO.MilestoneInTask()
                           {
                               Id = _dal.Task!.Read(_dal.Dependency!.Read(d =>
                               {
                                   _dal.Task!.Read(ta => ta.IsMilestone && ta.Id == d.DependentTask);
                                   return d.DependsOnTask == t.Id;
                               })!.DependentTask)!.Id,
                               Alias = _dal.Task!.Read(_dal.Dependency!.Read(d =>
                               {
                                   _dal.Task!.Read(ta => ta.IsMilestone && ta.Id == d.DependentTask);
                                   return d.DependsOnTask == t.Id;
                               })!.DependentTask)?.Alias
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
                task.Duration,
                task.CreatedAtDate,
                task.StartAtDate,
                task.ApproxStartAtDate,
                task.LastDateToEnd,
                task.EndAtDate,
                task.Deliverables,
                task.Remarks,
                task.Engineer?.Id,
                task.Level is not null ? (DO.EngineerExperience)task.Level : 0));
            _dal.Dependency!.ReadAll(d => d.DependentTask == task.Id && task.DependenciesList!.Any(de => d.DependsOnTask != de.Id))
                .ToList().ForEach(d => _dal.Dependency!.Delete(d.Id));
            task.DependenciesList!.ForEach(d =>
            {
                if (_dal.Dependency!.Read(de => de.DependentTask == task.Id && de.DependsOnTask == d.Id) is null)
                    _dal.Dependency!.Create(new DO.Dependency(0, task.Id, d.Id));
            });
            TopologicalSort(_dal.Dependency!.ReadAll()//send a graph to check that there are no circling dependencies
            .GroupBy(d => d.DependentTask).ToDictionary(
            group => group.Key,//dependent task
                group => group.Select(d => d.DependentTask).ToArray()//all dependencies for task
            ));
            _bl.Milestone!.UpdateMilestone(_dal.Task!.Read(_dal.Dependency!.Read(d =>
            {
                _dal.Task!.Read(ta => ta.IsMilestone && ta.Id == d.DependentTask);
                return d.DependsOnTask == task.Id;
            })!.DependentTask)!.Id);
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

    /// <summary>
    /// checking if there are any circling dependencies
    /// </summary>
    /// <param name="digraph"> digraph is a dictionary:
    /// key: a node(int)
    /// value: an array of adjacent nodes (array of ints)
    /// </param>
    /// <exception cref="BlCirclingDependenciesExeption"></exception>
    private static void TopologicalSort(Dictionary<int, int[]> digraph)
    {
        // Construct an dictionary mapping nodes to their indegrees
        var indegrees = new Dictionary<int, int>();
        foreach (var node in digraph.Keys)
        {
            indegrees.Add(node, 0);
        }
        foreach (var node in digraph.Keys)
        {
            foreach (var neighbor in digraph[node])
            {
                indegrees[neighbor] += 1;
            }
        }

        // Track nodes with no incoming edges
        var nodesWithNoIncomingEdges = new Queue<int>();
        foreach (var node in digraph.Keys)
        {
            if (indegrees[node] == 0)
            {
                nodesWithNoIncomingEdges.Enqueue(node);
            }
        }

        // Initially, no nodes in our ordering
        var topologicalOrdering = new List<int>();

        // As long as there are nodes with no incoming edges
        // that can be added to the ordering 
        while (nodesWithNoIncomingEdges.Count > 0)
        {

            // Add one of those nodes to the ordering
            var node = nodesWithNoIncomingEdges.Dequeue();
            topologicalOrdering.Add(node);

            // Decrement the indegree of that node's neighbors
            foreach (var neighbor in digraph[node])
            {
                indegrees[neighbor] -= 1;
                if (indegrees[neighbor] == 0)
                {
                    nodesWithNoIncomingEdges.Enqueue(neighbor);
                }
            }
        }

        // We've run out of nodes with no incoming edges
        // Did we add all the nodes or find a cycle?
        if (topologicalOrdering.Count != digraph.Count)
        {
            throw new BO.BlCirclingDependenciesExeption(" has a cycle! No topological ordering exists.");
        }
    }
}
