namespace DalTest;
using DO;
using Dal;
using System.Runtime;

internal class Program
{
    
    private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1;
    private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
    private static ITask? s_dalTask = new TaskImplementation(); //stage 1

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

    }
}
