using DO;

namespace DalTest;
public static class Initialization
{
    private static IDependency? s_dalDependency; //stage 1 40
    private static IEngineer? s_dalEngineer; //stage 1 5
    private static ITask? s_dalTask; //stage 1 20

    private static Random s_rand = new();

    private static void createTasks()
    {
        string[] taskDescriptions = {
            "Implementing Data Structures and Algorithms",
            "Developing Scalable Software Applications",
            "Creating RESTful APIs with ASP.NET Web API",
            "Database Management and Optimization",
            "Unit Testing and Debugging Codebase",
            "Implementing Real-time Systems and IoT Devices",
            "Optimizing Algorithm Performance with Multithreading",
            "Implementing Security Measures for Data Protection",
            "Integrating Third-party APIs and Libraries",
            "Developing User-friendly GUI Applications"
        };//size of 10

        string[] aliases = {
            "1 Algorithm Specialist",
            "2 Software Developer",
            "3 API Engineer",
            "4 Database Administrator",
            "5 Quality Assurance Engineer",
            "6 Real-time Systems Developer",
            "7 Multithreading Expert",
            "8 Security Engineer",
            "9 Integration Specialist",
            "10 GUI Developer"
        };//size of 10, maches taskDescriptions by index.

        string[] taskDeliverables = {
            "Project Proposal Document",
            "Functional Specifications",
            "Design Mockups and Wireframes",
            "Prototype Development",
            "Unit Test Cases",
            "Code Implementation",
            "Integration Testing",
            "User Documentation",
            "Deployment Package",
            "Quality Assurance Report"
        };

        string[] taskRemarks = {
            "Task completed successfully.",
            "Need additional resources for completion.",
            "Facing technical challenges, seeking help.",
            "Task postponed due to external dependencies.",
            "Completed ahead of schedule.",
            "Task in progress, making good progress.",
            "Waiting for client feedback.",
            "Task blocked, awaiting resolution.",
            "Revisiting requirements, task scope changed.",
            "Task completed with minor issues, pending review."
        };


        for (int i = 0; i < 20; i++)
        {//create task
            //makes an array of dates in cronoligical order
            DateTime[] dates = new DateTime[6];
            for (int j = 0; j < 6; j++)
            {
                if (j == 0)
                    dates[j] = new DateTime(s_rand.Next(2023, 2026), s_rand.Next(1, 13), s_rand.Next(1, 29));
                else
                    dates[j] = new DateTime(
                        s_rand.Next(dates[j - 1].Year, 2026),
                        s_rand.Next(dates[j - 1].Month, 13),
                        s_rand.Next(dates[j - 1].Day, 29));
            }
            int taskDescriptionIndex = s_rand.Next(0, 10);
            //no need to check if exists, because automatic id (config)
            DO.Task task = new DO.Task(
                0,
                taskDescriptions[taskDescriptionIndex],
                aliases[taskDescriptionIndex],
                (s_rand.Next(0, 2) == 1 ? true : false),
                dates[0], dates[1], dates[2], dates[3], dates[4], dates[5],
                taskDeliverables[taskDescriptionIndex],
                taskRemarks[taskDescriptionIndex],
                s_rand.Next(1, 6));
            s_dalTask.Create(task);
        }
    }

    private static void createEngineers()
    {
        string[] names = {
            "Alice",
            "Bob",
            "Charlie",
            "Diana",
            "Eva"
        };

        for (int i = 0; i < 5; i++)
        {
            int id = 0;
            do
                id = s_rand.Next(150000000, 400000000);
            while (s_dalEngineer.Read(id) is not null);
            Engineer engineer = new Engineer(
                id,
                names[i],
                $"{names[i]}@gmail.com",
                (DO.EngineerExperience)s_rand.Next(1, 6),
                (double)s_rand.Next(50, 200)
                );
            s_dalEngineer.Create(engineer);
        }
    }

    private static void createDependencies()
    {
        List<DO.Task>? tasks = s_dalTask.ReadAll();
        foreach (DO.Task task in tasks)
        {
            int taskId1 = task.Id;
            int taskId2 = tasks.Find(x => x.Alias[0] + 1 == task.Alias[0])?.Id ?? -1;
            int taskId3 = tasks.Find(x => x.Alias[0] + 2 == task.Alias[0])?.Id ?? -1;
            int taskId4 = tasks.Find(x => x.Alias[0] + 3 == task.Alias[0])?.Id ?? -1;
            if (taskId2 > -1)
            {
                DO.Dependency dependency = new Dependency(0, taskId1, taskId2);
                s_dalDependency.Create(dependency);
            }
            if (taskId3 > -1)
            {
                DO.Dependency dependency = new Dependency(0, taskId1, taskId3);
                s_dalDependency.Create(dependency);
            }
            if (taskId4 > -1)
            {
                DO.Dependency dependency = new Dependency(0, taskId1, taskId4);
                s_dalDependency.Create(dependency);
            }
        }
    }

    public static void Do(ITask? taskDal, IEngineer? engineerDal, IDependency? dependencyDal)
    {
        s_dalTask = taskDal ?? throw new Exception("DAL (task) can not be null!");
        s_dalEngineer = engineerDal ?? throw new Exception("DAL (engineer) can not be null!");
        s_dalDependency = dependencyDal ?? throw new Exception("DAL (dependency) can not be null!");
        createTasks();
        createEngineers();
        createDependencies();
    }
}
