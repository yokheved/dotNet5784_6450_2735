namespace BLTest;
using BlApi;
using BO;

internal class Program
{

    private static readonly IBl s_bl = Factory.Get;
    static void Main(string[] args)///Performs various actions according to the user's choice
    {
        try
        {
            Console.Write("Would you like to Reset data? (Y/N)"); //stage 3
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
            if (ans == "Y") //stage 3
                            //Initialization.Do(s_dal); //stage 2
                s_bl.Reset(); //stage 4
            Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
            ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
            if (ans == "Y") //stage 3
                            //Initialization.Do(s_dal); //stage 2
                DalTest.Initialization.Do(); //stage 4

            int? choice = 0;
            do
            {
                try
                {
                    Console.WriteLine("\n Enter your choice:");
                    choice = MainChoice();
                    switch (choice)
                    {
                        case 1:
                            ProjectCreate();
                            break;
                        case 2:
                            TaskHandle();
                            break;
                        case 3:
                            EngineerHandle();
                            break;
                        case 4:
                            MilestoneHandle();
                            break;
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            while (choice != 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        Console.WriteLine("Bye Bye!");
    }


    #region Task - option 2 in main
    private static void TaskHandle()///The TaskHandle function performs actions on tasks selected by the user.
    {
        CRUD? choice;
        choice = EntityChoice();
        switch (choice)
        {
            case CRUD.Create:
                TaskCreate();
                break;
            case CRUD.Read:
                TaskRead();
                break;
            case CRUD.ReadAll:
                TaskReadAll();
                break;
            case CRUD.Update:
                TaskUpdate();
                break;
            case CRUD.Delete:
                TaskDelete();
                break;
        }
    }
    private static Task TaskCreate(bool isProjectCreate = false)///creates a new task by receiving details from the user such as a description, alias and whether or not it is a milestone.
    {
        Console.WriteLine("Enter Task Description:");
        string? description = Console.ReadLine();

        Console.WriteLine("Enter Task Alias:");
        string? alias = Console.ReadLine();

        int days;
        Console.WriteLine("Enter duration of task (in days):");
        int.TryParse(Console.ReadLine(), out days);
        TimeSpan? duration = TimeSpan.FromDays(days);

        // For DateTime properties
        DateTime createdAtDate = DateTime.Now, scheduledDate = DateTime.MinValue, deadlineDate = DateTime.MinValue; // You can set the creation date based on your logic
        if (!isProjectCreate)
        {
            GetDateTimeFromUser("Enter Scheduled Date:", false, out scheduledDate);
            GetDateTimeFromUser("Enter Deadline Date:", false, out deadlineDate);
        }

        Console.WriteLine("Enter Deliverables:");
        string? deliverables = Console.ReadLine();

        Console.WriteLine("Enter Remarks:");
        string? remarks = Console.ReadLine();

        int engineerId;
        Console.WriteLine("Enter Task Engineer Id:");
        bool? succidedId = int.TryParse(Console.ReadLine(), out engineerId);

        Console.WriteLine("Enter Complexity Level (Novice, AdvancedBeginner, Competent, Proficient, Expert):");
        Enum.TryParse(Console.ReadLine(), out EngineerExperience complexityLevel);

        List<int> dependencies = new List<int>();
        List<TaskInList> dependenciesTask = new List<TaskInList>();
        int dependencyId = -1;
        if (isProjectCreate)
        {
            do
            {
                Console.WriteLine("enter task dependencyid:");
                int.TryParse(Console.ReadLine(), out dependencyId);
                if (dependencyId <= 0) break;
                dependencies.Add(dependencyId);
            } while (true);
            foreach (var t in s_bl.Task!.GetTasks())
            {
                if (dependencies.Contains(t.Id))
                    dependenciesTask.Add(new TaskInList()
                    {
                        Id = t.Id,
                        Alias = t.Alias,
                    });
            }
        }
        Task task = new Task()
        {
            Id = 1,
            Description = description,
            Alias = alias,
            Duration = duration,
            CreatedAtDate = createdAtDate,
            ApproxStartAtDate = scheduledDate == DateTime.MinValue ? scheduledDate : null,
            StartAtDate = null,
            LastDateToEnd = deadlineDate == DateTime.MinValue ? deadlineDate : null,
            EndAtDate = null,
            Status = 0,
            DependenciesList = dependenciesTask,
            Milestone = null,
            Deliverables = deliverables,
            Remarks = remarks,
            Engineer = new EngineerInTask() { Id = engineerId, Name = s_bl.Engineer!.GetEngineer(engineerId).Name },
            Level = complexityLevel
        };
        task = new Task()
        {
            Id = s_bl.Task!.AddTask(task),
            Description = description,
            Alias = alias,
            Duration = duration,
            CreatedAtDate = createdAtDate,
            ApproxStartAtDate = scheduledDate,
            StartAtDate = null,
            LastDateToEnd = deadlineDate,
            EndAtDate = null,
            Status = 0,
            DependenciesList = dependenciesTask,
            Milestone = null,
            Deliverables = deliverables,
            Remarks = remarks,
            Engineer = new EngineerInTask() { Id = engineerId, Name = s_bl.Engineer!.GetEngineer(engineerId).Name },
            Level = complexityLevel
        };
        return task;
    }
    private static void TaskRead()
    {
        Console.WriteLine("Enter Task ID to read:");
        int.TryParse(Console.ReadLine(), out int taskId);

        // Assuming s_dalTask.Read(taskId) method exists to read the task from your data source
        Task? task = s_bl!.Task!.GetTask(taskId);
        Console.WriteLine(task);
    }
    private static void TaskReadAll()
    {
        List<BO.Task> tasks = s_bl!.Task!.GetTasks().ToList();

        foreach (var task in tasks)
        {
            Console.WriteLine(task);
            Console.WriteLine("------------");
        }
    }
    private static void TaskUpdate()
    {
        Console.WriteLine("Enter Task ID to update:");
        if (!int.TryParse(Console.ReadLine(), out int taskId))
            throw new BlNotValidValueExeption("not valid input");

        Task? task = s_bl!.Task!.GetTask(taskId);
        Console.WriteLine("Current Task Details:");
        Console.WriteLine(task);

        Console.WriteLine("Enter Task Description:");
        string? description = Console.ReadLine();

        Console.WriteLine("Enter Task Alias:");
        string? alias = Console.ReadLine();

        int days;
        Console.WriteLine("Enter duration of task (in days):");
        bool isDuration = int.TryParse(Console.ReadLine(), out days);
        TimeSpan? duration = TimeSpan.FromDays(days);

        Console.WriteLine("Enter status: (Unscheduled, Scheduled, OnTrack, InJeopardy)");
        bool isStatus = int.TryParse(Console.ReadLine(), out int status);

        Console.WriteLine("Enter Deliverables:");
        string? deliverables = Console.ReadLine();

        Console.WriteLine("Enter Remarks:");
        string? remarks = Console.ReadLine();

        int engineerId;
        Console.WriteLine("Enter Task Engineer Id:");
        bool succidedId = int.TryParse(Console.ReadLine(), out engineerId);

        Console.WriteLine("Enter Complexity Level (Novice, Intermediate, or Expert):");
        bool levelSucceeded = Enum.TryParse<EngineerExperience>(Console.ReadLine(), out EngineerExperience complexityLevel);

        List<TaskInList> dependencies = (from d in task.DependenciesList select d).ToList();
        int dependencyId = 0;
        do
        {
            Console.WriteLine("enter task dependencyid (if non. enter -1):");
            int.TryParse(Console.ReadLine(), out dependencyId);
            if (dependencyId <= 0) break;
            Task dTask = s_bl.Task!.GetTask(dependencyId);
            if (!dependencies.Any(d => d.Id == dependencyId))
                dependencies.Add(new TaskInList()
                { Id = dependencyId, Alias = dTask.Alias, Description = dTask.Description, Status = dTask.Status });
        } while (true);

        Task taskUpdate = new Task()
        {
            Id = task.Id,
            Description = description ?? task.Description,
            Alias = alias == "" ? task.Alias : alias,
            Duration = duration ?? task.Duration,
            CreatedAtDate = task.CreatedAtDate,
            ApproxStartAtDate = task.ApproxStartAtDate,
            StartAtDate = status == 1 ? DateTime.Now : task.StartAtDate,
            LastDateToEnd = task.LastDateToEnd,
            EndAtDate = status == 3 ? DateTime.Now : task.EndAtDate,
            Status = isStatus ? (Status)status : task.Status,
            DependenciesList = dependencies,
            Deliverables = deliverables ?? task.Deliverables,
            Remarks = remarks ?? task.Remarks,
            Engineer = succidedId ? new EngineerInTask() { Id = engineerId, Name = s_bl.Engineer!.GetEngineer(engineerId).Name } : task.Engineer,
            Level = levelSucceeded ? complexityLevel : task.Level
        };

        // Update the task using s_blTask.Update(task)
        s_bl!.Task.UpdateTask(taskUpdate);
    }
    private static void TaskDelete()
    {
        Console.WriteLine("Enter Task ID to delete:");
        int taskId = int.Parse(Console.ReadLine());

        // Delete the task using s_blTask.DeleteTask(taskId)
        s_bl!.Task!.DeleteTask(taskId);
        Console.WriteLine("Task deleted successfully.");
    }
    #endregion

    #region Engineer - option 3 in main
    private static void EngineerHandle()///The EngineerHandle function performs actions on engineers of the user's choice.
    {
        CRUD? choice;
        choice = EntityChoice();
        switch (choice)
        {
            case CRUD.Create:
                EngineerCreate();
                break;
            case CRUD.Read:
                EngineerRead();
                break;
            case CRUD.ReadAll:
                EngineerReadAll();
                break;
            case CRUD.Update:
                EngineerUpdate();
                break;
            case CRUD.Delete:
                EngineerDelete();
                break;
        }
    }
    private static void EngineerCreate()
    {
        int engineerId;
        Console.WriteLine("Enter Engineer Id:");
        int.TryParse(Console.ReadLine(), out engineerId);

        Console.WriteLine("Enter Engineer Name:");
        string? name = Console.ReadLine();

        Console.WriteLine("Enter Engineer Email:");
        string? email = Console.ReadLine();

        Console.WriteLine("Enter Engineer Level (Novice, AdvancedBeginner, Competent, Proficient, Expert):");
        EngineerExperience level = Enum.Parse<EngineerExperience>(Console.ReadLine());

        Console.WriteLine("Enter Engineer Cost:");
        double cost = double.Parse(Console.ReadLine());

        Engineer engineer = new Engineer() { Id = engineerId, Name = name, Email = email, Level = level, Cost = cost };
        s_bl.Engineer.AddEngineer(engineer);
    }
    private static void EngineerRead()
    {
        Console.WriteLine("Enter Engineer ID to read:");
        int engineerId = int.Parse(Console.ReadLine());

        // Assuming s_dalEngineer.Read(engineerId) method exists to read the engineer from your data source
        Engineer? engineer = s_bl!.Engineer!.GetEngineer(engineerId);

        Console.WriteLine(engineer);
    }
    private static void EngineerReadAll()
    {
        List<Engineer> engineers = s_bl!.Engineer!.GetEngineerList().ToList();

        foreach (var engineer in engineers)
        {
            Console.WriteLine(engineer);
            Console.WriteLine("------------");
        }
    }
    private static void EngineerUpdate()
    {
        Console.WriteLine("Enter Engineer ID to update:");
        int engineerId = int.Parse(Console.ReadLine());

        Engineer? engineer = s_bl!.Engineer!.GetEngineer(engineerId);
        Console.WriteLine("Current Engineer Details:");
        Console.WriteLine(engineer);

        Console.WriteLine("Enter Engineer Name:");
        string? name = Console.ReadLine();

        Console.WriteLine("Enter Engineer Email:");
        string? email = Console.ReadLine();

        Console.WriteLine("Enter Engineer Level (Novice, AdvancedBeginner, Competent, Proficient, Expert):");
        bool levelSucceeded = Enum.TryParse<EngineerExperience>(Console.ReadLine(), out EngineerExperience level);

        Console.WriteLine("Enter Engineer Cost:");
        bool successCost = double.TryParse(Console.ReadLine(), out double cost);

        Engineer engineerUpdate = new Engineer()
        {
            Id = engineerId,
            Name = name ?? engineer!.Name,
            Email = email ?? engineer!.Email,
            Level = levelSucceeded ? level : engineer!.Level,
            Cost = successCost ? cost : engineer!.Cost
        };

        // Update the engineer using s_blEngineer.Update(engineer)
        s_bl!.Engineer!.UpdateEngineer(engineerUpdate);
    }
    private static void EngineerDelete()
    {
        Console.WriteLine("Enter Engineer ID to delete:");
        int engineerId = int.Parse(Console.ReadLine());

        // Delete the engineer using s_dalEngineer.Delete(engineerId)
        s_bl!.Engineer!.DeleteEgineer(engineerId);
        Console.WriteLine("Engineer deleted successfully.");
    }

    #endregion

    #region Milestone - option 4 in main
    private static void MilestoneHandle()///performs actions on dependencies of the user's choice.
    {
        CRUD? choice;
        choice = EntityChoice(true);
        switch (choice)
        {
            case CRUD.Read:
                MilestoneRead();
                break;
            case CRUD.Update:
                MilestoneUpdate();
                break;
            default:
                break;
        }
    }
    private static void MilestoneRead()
    {
        Console.WriteLine("Enter Dependency ID to read:");
        int milestoneId = int.Parse(Console.ReadLine());

        // Assuming s_bl.Milestone.GetMilestone method exists to read the milestone from your data source
        Milestone? dependency = s_bl!.Milestone!.GetMilestone(milestoneId);

        Console.WriteLine(dependency);
    }
    private static void MilestoneUpdate()
    {
        Console.WriteLine("Enter Milestone ID to update:");
        int milestoneId = int.Parse(Console.ReadLine());

        Milestone? milestone = s_bl!.Milestone!.GetMilestone(milestoneId);
        Console.WriteLine("Current Milestone Details:");
        Console.WriteLine(milestone);

        Console.WriteLine("Enter Milestone Alias:");
        string? alias = Console.ReadLine();

        Console.WriteLine("Enter Milestone Description:");
        string? description = Console.ReadLine();

        Console.WriteLine("Enter Milestone Remarks:");
        string? remarks = Console.ReadLine();
        // Update the dependency using s_dalDependency.Update(dependency)
        s_bl!.Milestone!.UpdateMilestone(milestoneId, alias, description, remarks);
    }
    #endregion

    #region additional functions
    /// <summary>
    /// gets choice of action from the user : create, read, read all, update, delete
    /// </summary>
    /// <returns>returns type CRUD enum, users choice</returns>
    private static CRUD EntityChoice(bool isMilestone = false)///accepts a choice from a menu from the user and returns the choice as a CRUD value.
    {
        int choice;
        if (!isMilestone) Console.WriteLine("create: press 0");
        Console.WriteLine("read: press 1");
        if (!isMilestone) Console.WriteLine("read all: press 2");
        Console.WriteLine("update: press 3");
        if (!isMilestone) Console.WriteLine("delete: press 4");
        int.TryParse(Console.ReadLine(), out choice);
        return (CRUD)choice;
    }
    /// <summary>
    /// gets useres date time object inputed
    /// </summary>
    /// <param name="prompt">message to show befor input as label</param>
    /// <param name="allowNull">bool if should allow user give no input</param>
    /// <returns>returns date time object from user, if no inputt returns null</returns>
    private static bool GetDateTimeFromUser(string prompt, bool allowNull, out DateTime date)
    {
        Console.WriteLine(prompt);
        bool success = DateTime.TryParse(Console.ReadLine(), out date);
        return success || !allowNull;
    }
    /// <summary>
    /// gets action from user, which entity to deal with
    /// </summary>
    /// <returns>int choice from user</returns>
    private static int? MainChoice()
    {
        Console.WriteLine("project create: press 1");
        Console.WriteLine("task: press 2");
        Console.WriteLine("engineer: press 3");
        Console.WriteLine("milestone: press 4");
        Console.WriteLine("exit: press 0");
        int choice = int.Parse(Console.ReadLine()!);
        return choice;
    }
    private static void ProjectCreate()
    {
        List<Task> tasks = new List<Task>();
        while (true)
        {
            Console.WriteLine("To enter task for project: (Y/N)");
            string? ans = Console.ReadLine() ?? throw new BlNotValidValueExeption("non valid value");
            if (ans == "Y")
            {
                Task task = TaskCreate(true);
                tasks.Add(task);
                Console.WriteLine($"task with id {task.Id} was added successfully");
            }
            else break;
        }
        if (tasks.Count == 0) throw new BlNotValidValueExeption("can't create a project with no tasks");
        GetDateTimeFromUser("Enter start date for project:", false, out DateTime startDate);
        GetDateTimeFromUser("Enter deadline date for project:", false, out DateTime deadlineDate);
        s_bl.Milestone!.CreateProjectSchedule(startDate, deadlineDate, tasks);
    }

    #endregion
}