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
            Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
            if (ans == "Y") //stage 3
                            //Initialization.Do(s_dal); //stage 2
                DalTest.Initialization.Do(); //stage 4

            int? choice;
            do
            {
                Console.WriteLine("\n Enter your choice:");
                choice = MainChoice();
                switch (choice)
                {
                    case 1:
                        TaskHandle();
                        break;
                    case 2:
                        EngineerHandle();
                        break;
                    case 3:
                        MilestoneHandle();
                        break;
                }
            }
            while (choice != 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        Console.WriteLine("Bye Bye!");
    }


    #region Task - option 1 in main
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
    private static Task TaskCreate()///creates a new task by receiving details from the user such as a description, alias and whether or not it is a milestone.
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
        DateTime createdAtDate = DateTime.Now; // You can set the creation date based on your logic
        GetDateTimeFromUser("Enter Scheduled Date:", false, out DateTime scheduledDate);
        GetDateTimeFromUser("Enter Deadline Date:", false, out DateTime deadlineDate);

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
        int dependencyId = -1;
        do
        {
            Console.WriteLine("enter task dependencyid (if non. enter -1):");
            int.TryParse(Console.ReadLine(), out dependencyId);
            if (dependencyId == -1) break;
            dependencies.Add(dependencyId);
        } while (true);
        List<TaskInList> dependenciesTask = new List<TaskInList>();
        foreach (var t in s_bl.Task!.GetTasks())
        {
            if (dependencies.Contains(t.Id))
                dependenciesTask.Add(new TaskInList()
                {
                    Id = t.Id,
                    Alias = t.Alias,
                });
        }
        Task task = new Task()
        {
            Id = 0,
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
        s_bl.Task!.AddTask(task);
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
            if (dependencyId == -1) break;
            Task dTask = s_bl.Task!.GetTask(dependencyId);
            if (!dependencies.Any(d => d.Id == dependencyId))
                dependencies.Add(new TaskInList()
                { Id = dependencyId, Alias = dTask.Alias, Description = dTask.Description, Status = dTask.Status });
        } while (true);

        Task taskUpdate = new Task()
        {
            Id = task.Id,
            Description = description ?? task.Description,
            Alias = alias ?? task.Alias,
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

        // Delete the task using s_dalTask.Delete(taskId)
        s_bl!.Task!.DeleteTask(taskId);
        Console.WriteLine("Task deleted successfully.");
    }
    #endregion

    #region Engineer - option 2 in main
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

    #region Dependency - option 3 in main
    private static void MilestoneHandle()///performs actions on dependencies of the user's choice.
    {
        CRUD? choice;
        choice = EntityChoice();
        switch (choice)
        {
            case CRUD.Read:
                MilestoneRead();
                break;
            case CRUD.ReadAll:
                MilestoneReadAll();
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
        int dependencyId = int.Parse(Console.ReadLine());

        // Assuming s_dalDependency.Read(dependencyId) method exists to read the dependency from your data source
        Dependency? dependency = s_dal!.Dependency!.Read(dependencyId);

        Console.WriteLine(dependency);
    }
    private static void MilestoneReadAll()
    {
        List<Dependency> dependencies = s_dal!.Dependency!.ReadAll().ToList();

        foreach (var dependency in dependencies)
        {
            Console.WriteLine(dependency);
            Console.WriteLine("------------");
        }
    }
    private static void MilestoneUpdate()
    {
        Console.WriteLine("Enter Dependency ID to update:");
        int dependencyId = int.Parse(Console.ReadLine());

        Dependency? dependency = s_dal!.Dependency!.Read(dependencyId);
        Console.WriteLine("Current Dependency Details:");
        Console.WriteLine(dependency);

        Console.WriteLine("Enter Dependent Task ID:");
        bool dependentSucceed = int.TryParse(Console.ReadLine(), out int dependentTask);

        Console.WriteLine("Enter Depends On Task ID:");
        bool dependentOnSucceed = int.TryParse(Console.ReadLine(), out int dependsOnTask);
        // Update the dependency using s_dalDependency.Update(dependency)
        s_dal!.Dependency!.Update(new DO.Dependency(
            dependency!.Id,
            dependentSucceed ? dependentTask : dependency.DependentTask,
            dependentOnSucceed ? dependsOnTask : dependency.DependsOnTask)
        );
    }
    #endregion

    #region additional functions
    /// <summary>
    /// gets choice of action from the user : create, read, read all, update, delete
    /// </summary>
    /// <returns>returns type CRUD enum, users choice</returns>
    private static CRUD EntityChoice()///accepts a choice from a menu from the user and returns the choice as a CRUD value.
    {
        int choice;
        Console.WriteLine("create: press 0");
        Console.WriteLine("read: press 1");
        Console.WriteLine("read all: press 2");
        Console.WriteLine("update: press 3");
        Console.WriteLine("delete: press 4");

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
            if (ans == "Y") tasks.Add(TaskCreate());
            else break;
        }
        if (tasks.Count == 0) throw new BlNotValidValueExeption("can't create a project with no tasks");
        GetDateTimeFromUser("Enter start date for project:", false, out DateTime startDate);
        GetDateTimeFromUser("Enter deadline date for project:", false, out DateTime deadlineDate);
        s_bl.Milestone!.CreateProjectSchedule(startDate, deadlineDate, tasks);
    }

    #endregion
}