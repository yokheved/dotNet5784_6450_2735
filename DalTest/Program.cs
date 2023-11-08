namespace DalTest;
using DO;
using Dal;
using System.Runtime;
using System.Transactions;

internal class Program
{
    
    private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1;
    private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
    private static ITask? s_dalTask = new TaskImplementation(); //stage 1
    private static Random s_rand = new Random();
    static void Main(string[] args)
    {
        Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency);
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
        Console.WriteLine("Bye Bye!");
    }
    private static int? MainChoice()
    {
        int choice;
        Console.WriteLine("task: press 1");
        Console.WriteLine("engineer: press 2");
        Console.WriteLine("dependency: press 3");
        Console.WriteLine("exit: press 0");
        int.TryParse(Console.ReadLine(), out choice);
        return choice;  
    }
    private static void TaskHandle()
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
    private static void EngineerHandle() 
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
    private static void DependencyHandle()
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
    private static CRUD EntityChoice()
    {
        int choice;
        Console.WriteLine("create: press 1");
        Console.WriteLine("read: press 2");
        Console.WriteLine("read all: press 3");
        Console.WriteLine("update: press 4");
        Console.WriteLine("delete: press 5");

        int.TryParse(Console.ReadLine(), out choice);
        return (CRUD)choice;
    }
    private static void TaskCreate()
    {
        Console.WriteLine("Enter Task Description:");
        string? description = Console.ReadLine();

        Console.WriteLine("Enter Task Alias:");
        string? alias = Console.ReadLine();

        Console.WriteLine("Is it a milestone? (true or false):");
        bool isMilestone = bool.Parse(Console.ReadLine());

        // For DateTime properties 
        DateTime startDate = GetDateTimeFromUser("Enter Start Date:");
        DateTime scheduledDate = GetDateTimeFromUser("Enter Scheduled Date:");
        DateTime forecastDate = GetDateTimeFromUser("Enter Forecast Date:");
        DateTime deadlineDate = GetDateTimeFromUser("Enter Deadline Date:");
        DateTime completeDate = GetDateTimeFromUser("Enter Complete Date:");

        Console.WriteLine("Enter Deliverables:");
        string? deliverables = Console.ReadLine();

        Console.WriteLine("Enter Remarks:");
        string? remarks = Console.ReadLine();

        List<DO.Engineer> engineers = s_dalEngineer.ReadAll();
        int engineerId = engineers[s_rand.Next(0, engineers.Count)].Id;

        Console.WriteLine("Enter Complexity Level (Novice, Intermediate, or Expert):");
        EngineerExperience complexityLevel = Enum.Parse<EngineerExperience>(Console.ReadLine());

        // Create the Task object using the provided input
        Task task = new Task(
            0,
            description,
            alias,
            isMilestone,
            null, // CreatedAtDate (can be set to null or initialized based on your logic)
            startDate,
            scheduledDate,
            forecastDate,
            deadlineDate,
            completeDate,
            deliverables,
            remarks,
            engineerId,
            complexityLevel
        );
        s_dalTask.Create(task);
        
    }
    private static DateTime GetDateTimeFromUser(string prompt)
    {
        Console.WriteLine(prompt);
        while (true)
        {
            if (DateTime.TryParse(Console.ReadLine(), out DateTime result))
            {
                return result;
            }
            else
            {
                Console.WriteLine("Invalid date format. Please try again.");
            }
        }
    }
    private static void EngineerCreate()
    {
        int engineerId;
        Console.WriteLine("Enter Engineer Name:");
        int.TryParse(Console.ReadLine(), out engineerId);

        Console.WriteLine("Enter Engineer Name:");
        string? name = Console.ReadLine();

        Console.WriteLine("Enter Engineer Email:");
        string? email = Console.ReadLine();

        Console.WriteLine("Enter Engineer Level (Novice, AdvancedBeginner, Competent Proficient, Expert):");
        EngineerExperience level = Enum.Parse<EngineerExperience>(Console.ReadLine());

        Console.WriteLine("Enter Engineer Cost:");
        double? cost = double.Parse(Console.ReadLine());

        // Create the Engineer object using the provided input
        Engineer engineer = new Engineer(
            engineerId,
            name,
            email,
            level,
            cost
        );

        // Assuming s_dalEngineer.Create() method exists to add the engineer to your data source
        s_dalEngineer.Create(engineer);
    }
    private static void DependencyCreate()
    {
        Console.WriteLine("Enter Dependent Task ID:");
        int dependentTaskId = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter Depends On Task ID:");
        int dependsOnTaskId = int.Parse(Console.ReadLine());

        // Create the Dependency object using the provided input
        Dependency dependency = new Dependency(
            0,
            dependentTaskId,
            dependsOnTaskId
        );

        // Assuming s_dalDependency.Create() method exists to add the dependency to your data source
        s_dalDependency.Create(dependency);
    }
    private static void TaskRead()
    {
        Console.WriteLine("Enter Task ID to read:");
        int taskId = int.Parse(Console.ReadLine());

        // Assuming s_dalTask.Read(taskId) method exists to read the task from your data source
        DO.Task task = s_dalTask.Read(taskId);
        Console.WriteLine($"Task ID: {task.Id}");
        Console.WriteLine($"Description: {task.Discription}");
        Console.WriteLine($"Alias: {task.Alias}");
        Console.WriteLine($"Is Milestone: {task.IsMilestone}");
        Console.WriteLine($"Created At Date: {task.CreatedAtDate}");
        Console.WriteLine($"Start Date: {task.StartDate}");
        Console.WriteLine($"Scheduled Date: {task.ScheduledDate}");
        Console.WriteLine($"Forecast Date: {task.ForecastDate}");
        Console.WriteLine($"Deadline Date: {task.DeadlineDate}");
        Console.WriteLine($"Complete Date: {task.CompleteDate}");
        Console.WriteLine($"Deliverables: {task.Deliverables}");
        Console.WriteLine($"Remarks: {task.Remarks}");
        Console.WriteLine($"Engineer ID: {task.EngineerId}");
        Console.WriteLine($"Complexity Level: {task.ComplexityLevel}");
        Console.WriteLine("Task read successfully.");
    }
    private static void EngineerRead()
    {
        Console.WriteLine("Enter Engineer ID to read:");
        int engineerId = int.Parse(Console.ReadLine());

        // Assuming s_dalEngineer.Read(engineerId) method exists to read the engineer from your data source
        DO.Engineer engineer = s_dalEngineer.Read(engineerId);

        Console.WriteLine($"Engineer ID: {engineer.Id}");
        Console.WriteLine($"Name: {engineer.Name}");
        Console.WriteLine($"Email: {engineer.Email}");
        Console.WriteLine($"Level: {engineer.Level}");
        Console.WriteLine($"Cost: {engineer.Cost}");
    }
    private static void DependencyRead()
    {
        Console.WriteLine("Enter Dependency ID to read:");
        int dependencyId = int.Parse(Console.ReadLine());

        // Assuming s_dalDependency.Read(dependencyId) method exists to read the dependency from your data source
        DO.Dependency dependency = s_dalDependency.Read(dependencyId);

        Console.WriteLine($"Dependency ID: {dependency.Id}");
        Console.WriteLine($"Dependent Task ID: {dependency.DependentTask}");
        Console.WriteLine($"Depends On Task ID: {dependency.DependsOnTask}");
    }
    private static void TaskReadAll()
    {
        List<DO.Task> tasks = s_dalTask.ReadAll();

        foreach (var task in tasks)
        {
            Console.WriteLine($"Task ID: {task.Id}");
            Console.WriteLine($"Description: {task.Discription}");
            Console.WriteLine($"Alias: {task.Alias}");
            Console.WriteLine($"Is Milestone: {task.IsMilestone}");
            Console.WriteLine($"Created At Date: {task.CreatedAtDate}");
            Console.WriteLine($"Start Date: {task.StartDate}");
            Console.WriteLine($"Scheduled Date: {task.ScheduledDate}");
            Console.WriteLine($"Forecast Date: {task.ForecastDate}");
            Console.WriteLine($"Deadline Date: {task.DeadlineDate}");
            Console.WriteLine($"Complete Date: {task.CompleteDate}");
            Console.WriteLine($"Deliverables: {task.Deliverables}");
            Console.WriteLine($"Remarks: {task.Remarks}");
            Console.WriteLine($"Engineer ID: {task.EngineerId}");
            Console.WriteLine($"Complexity Level: {task.ComplexityLevel}");
            Console.WriteLine("------------");
        }
    }
    private static void EngineerReadAll()
    {
        List<DO.Engineer> engineers = s_dalEngineer.ReadAll();

        foreach (var engineer in engineers)
        {
            Console.WriteLine($"Engineer ID: {engineer.Id}");
            Console.WriteLine($"Name: {engineer.Name}");
            Console.WriteLine($"Email: {engineer.Email}");
            Console.WriteLine($"Level: {engineer.Level}");
            Console.WriteLine($"Cost: {engineer.Cost}");
            Console.WriteLine("------------");
        }
    }
    private static void DependencyReadAll()
    {
        List<DO.Dependency> dependencies = s_dalDependency.ReadAll();

        foreach (var dependency in dependencies)
        {
            Console.WriteLine($"Dependency ID: {dependency.Id}");
            Console.WriteLine($"Dependent Task ID: {dependency.DependentTask}");
            Console.WriteLine($"Depends On Task ID: {dependency.DependsOnTask}");
            Console.WriteLine("------------");
        }
    }
}
הי יוכי שיהיה בהצלחה רבההההההההה

