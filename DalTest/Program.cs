namespace DalTest;
using Dal;
using DO;
internal class Program
{

    //static readonly IDal s_dal = new DalList(); //stage 2 
    static readonly IDal s_dal = new DalXml();
    private static Random s_rand = new Random();
    static void Main(string[] args)///Performs various actions according to the user's choice
    {
        try
        {
            Initialization.Do(s_dal);
            Console.WriteLine("\n Enter your choice:");
            int? choice;
            do
            {
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
                        DependencyHandle();
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
    private static void TaskCreate()///creates a new task by receiving details from the user such as a description, alias and whether or not it is a milestone.
    {
        Console.WriteLine("Enter Task Description:");
        string? description = Console.ReadLine();

        Console.WriteLine("Enter Task Alias:");
        string? alias = Console.ReadLine();

        Console.WriteLine("Is it a milestone? (true or false):");
        bool isMilestone;
        bool.TryParse(Console.ReadLine(), out isMilestone);

        // For DateTime properties
        DateTime createdAtDate = DateTime.Now; // You can set the creation date based on your logic
        DateTime? startDate = GetDateTimeFromUser("Enter Start Date:", false);
        DateTime? scheduledDate = GetDateTimeFromUser("Enter Scheduled Date:", false);
        DateTime? forecastDate = GetDateTimeFromUser("Enter Forecast Date:", false);
        DateTime? deadlineDate = GetDateTimeFromUser("Enter Deadline Date:", false);
        DateTime? completeDate = GetDateTimeFromUser("Enter Complete Date:", false);

        Console.WriteLine("Enter Deliverables:");
        string? deliverables = Console.ReadLine();

        Console.WriteLine("Enter Remarks:");
        string? remarks = Console.ReadLine();

        int engineerId;
        Console.WriteLine("Enter Task Engineer Id:");
        bool? succidedId = int.TryParse(Console.ReadLine(), out engineerId);

        Console.WriteLine("Enter Complexity Level (Novice, AdvancedBeginner, Competent, Proficient, Expert):");
        Enum.TryParse<EngineerExperience>(Console.ReadLine(), out EngineerExperience complexityLevel);

        Task task = new Task(
            0,
            description,
            alias,
            isMilestone,
            createdAtDate,
            (DateTime)startDate,
            (DateTime)scheduledDate,
            (DateTime)forecastDate,
            (DateTime)deadlineDate,
            (DateTime)completeDate,
            deliverables,
            remarks,
            engineerId,
            complexityLevel
        );
        s_dal!.Task!.Create(task);
    }
    private static void TaskRead()
    {
        Console.WriteLine("Enter Task ID to read:");
        int taskId = int.Parse(Console.ReadLine());

        // Assuming s_dalTask.Read(taskId) method exists to read the task from your data source
        Task? task = s_dal!.Task!.Read(taskId);
        Console.WriteLine(task);
    }
    private static void TaskReadAll()
    {
        List<DO.Task> tasks = s_dal!.Task!.ReadAll().ToList();

        foreach (var task in tasks)
        {
            Console.WriteLine(task);
            Console.WriteLine("------------");
        }
    }
    private static void TaskUpdate()
    {
        Console.WriteLine("Enter Task ID to update:");
        int taskId = int.Parse(Console.ReadLine());

        Task? task = s_dal!.Task!.Read(taskId);
        Console.WriteLine("Current Task Details:");
        Console.WriteLine(task);

        Console.WriteLine("Enter Task Description:");
        string? description = Console.ReadLine();

        Console.WriteLine("Enter Task Alias:");
        string? alias = Console.ReadLine();

        Console.WriteLine("Is it a milestone? (true or false):");
        bool isMilestone;
        bool succidedIs = bool.TryParse(Console.ReadLine(), out isMilestone);

        // For DateTime properties
        DateTime createdAtDate = task.CreatedAtDate; // You can set the creation date based on your logic
        DateTime? startDate = GetDateTimeFromUser("Enter Start Date:", true);
        DateTime? scheduledDate = GetDateTimeFromUser("Enter Scheduled Date:", true);
        DateTime? forecastDate = GetDateTimeFromUser("Enter Forecast Date:", true);
        DateTime? deadlineDate = GetDateTimeFromUser("Enter Deadline Date:", true);
        DateTime? completeDate = GetDateTimeFromUser("Enter Complete Date:", true);

        Console.WriteLine("Enter Deliverables:");
        string? deliverables = Console.ReadLine();

        Console.WriteLine("Enter Remarks:");
        string? remarks = Console.ReadLine();

        int engineerId;
        Console.WriteLine("Enter Task Engineer Id:");
        bool succidedId = int.TryParse(Console.ReadLine(), out engineerId);

        Console.WriteLine("Enter Complexity Level (Novice, Intermediate, or Expert):");
        bool levelSucceeded = Enum.TryParse<EngineerExperience>(Console.ReadLine(), out EngineerExperience complexityLevel);

        DO.Task taskUpdate = new DO.Task(
            task.Id,
            description ?? task.Discription,
            alias ?? task.Alias,
            succidedIs ? isMilestone : task.IsMilestone,
            createdAtDate,
            startDate is not null ? (DateTime)startDate : task.StartDate,
            scheduledDate is not null ? (DateTime)scheduledDate : task.ScheduledDate,
            forecastDate is not null ? (DateTime)forecastDate : task.ForecastDate,
            deadlineDate is not null ? (DateTime)deadlineDate : task.DeadlineDate,
            completeDate is not null ? (DateTime)completeDate : task.CompleteDate,
            deliverables ?? task.Deliverables,
            remarks ?? task.Remarks,
            succidedId ? engineerId : task.EngineerId,
            levelSucceeded ? complexityLevel : task.ComplexityLevel
        );

        // Update the task using s_dalTask.Update(task)
        s_dal!.Task.Update(taskUpdate);
    }
    private static void TaskDelete()
    {
        Console.WriteLine("Enter Task ID to delete:");
        int taskId = int.Parse(Console.ReadLine());

        // Delete the task using s_dalTask.Delete(taskId)
        s_dal!.Task!.Delete(taskId);
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
        double? cost = double.Parse(Console.ReadLine());

        Engineer engineer = new Engineer(engineerId, name, email, level, cost);
        s_dal!.Engineer!.Create(engineer);
    }
    private static void EngineerRead()
    {
        Console.WriteLine("Enter Engineer ID to read:");
        int engineerId = int.Parse(Console.ReadLine());

        // Assuming s_dalEngineer.Read(engineerId) method exists to read the engineer from your data source
        Engineer? engineer = s_dal!.Engineer!.Read(engineerId);

        Console.WriteLine(engineer);
    }
    private static void EngineerReadAll()
    {
        List<Engineer> engineers = s_dal!.Engineer!.ReadAll().ToList();

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

        Engineer? engineer = s_dal!.Engineer!.Read(engineerId);
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

        DO.Engineer engineerUpdate = new DO.Engineer(
            engineerId,
            name ?? engineer!.Name,
            email ?? engineer!.Email,
            levelSucceeded ? level : engineer!.Level,
            successCost ? cost : engineer!.Cost);

        // Update the engineer using s_dalEngineer.Update(engineer)
        s_dal!.Engineer!.Update(engineerUpdate);
    }
    private static void EngineerDelete()
    {
        Console.WriteLine("Enter Engineer ID to delete:");
        int engineerId = int.Parse(Console.ReadLine());

        // Delete the engineer using s_dalEngineer.Delete(engineerId)
        s_dal!.Engineer!.Delete(engineerId);
        Console.WriteLine("Engineer deleted successfully.");
    }

    #endregion

    #region Dependency - option 3 in main
    private static void DependencyHandle()///performs actions on dependencies of the user's choice.
    {
        CRUD? choice;
        choice = EntityChoice();
        switch (choice)
        {
            case CRUD.Create:
                DependencyCreate();
                break;
            case CRUD.Read:
                DependencyRead();
                break;
            case CRUD.ReadAll:
                DependencyReadAll();
                break;
            case CRUD.Update:
                DependencyUpdate();
                break;
            case CRUD.Delete:
                DependencyDelete();
                break;
        }
    }
    private static void DependencyCreate()
    {
        Console.WriteLine("Enter Dependent Task ID:");
        int dependentTask = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter Depends On Task ID:");
        int dependsOnTask = int.Parse(Console.ReadLine());

        s_dal!.Dependency!.Create(new DO.Dependency(0, dependentTask, dependsOnTask));
    }
    private static void DependencyRead()
    {
        Console.WriteLine("Enter Dependency ID to read:");
        int dependencyId = int.Parse(Console.ReadLine());

        // Assuming s_dalDependency.Read(dependencyId) method exists to read the dependency from your data source
        Dependency? dependency = s_dal!.Dependency!.Read(dependencyId);

        Console.WriteLine(dependency);
    }
    private static void DependencyReadAll()
    {
        List<Dependency> dependencies = s_dal!.Dependency!.ReadAll().ToList();

        foreach (var dependency in dependencies)
        {
            Console.WriteLine(dependency);
            Console.WriteLine("------------");
        }
    }
    private static void DependencyUpdate()
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
    private static void DependencyDelete()
    {
        Console.WriteLine("Enter Dependency ID to delete:");
        int dependencyId = int.Parse(Console.ReadLine());

        // Delete the dependency using s_dalDependency.Delete(dependencyId)
        s_dal!.Dependency!.Delete(dependencyId);
        Console.WriteLine("Dependency deleted successfully.");
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
    private static DateTime? GetDateTimeFromUser(string prompt, bool allowNull)
    {
        Console.WriteLine(prompt);
        bool success = DateTime.TryParse(Console.ReadLine(), out DateTime result);
        return !success && allowNull ? null : result;
    }
    /// <summary>
    /// gets action from user, which entity to deal with
    /// </summary>
    /// <returns>int choice from user</returns>
    private static int? MainChoice()
    {
        Console.WriteLine("task: press 1");
        Console.WriteLine("engineer: press 2");
        Console.WriteLine("dependency: press 3");
        Console.WriteLine("exit: press 0");
        int choice = int.Parse(Console.ReadLine()!);
        return choice;
    }

    #endregion
}