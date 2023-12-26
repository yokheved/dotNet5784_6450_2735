﻿using DO;

namespace DalTest;
public static class Initialization
{
    private static IDal? s_dal; //stage 2 

    private static Random s_rand = new();

    private static void createTasks()
    {
        ///An array of strings that describe different tasks
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
        ///Strings representing various descriptions or nicknames for positions in the field of programming and engineering
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
            DateTime[] dates = new DateTime[5];
            for (int j = 0; j < 5; j++)
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
                TimeSpan.FromDays(s_rand.Next(2, 11)),
                dates[0], dates[1], dates[2], dates[3], dates[4],
                taskDeliverables[taskDescriptionIndex],
                taskRemarks[taskDescriptionIndex],
                s_rand.Next(1, 6));
            s_dal!.Task!.Create(task);
        }
    }

    /// <summary>
    /// creates and initializes an array of 5 engineer names. It then generates a unique ID number for each engineer and creates a new Engineer object with the ID number, name, and email address.
    /// </summary>
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
            {
                id = s_rand.Next(150000000, 400000000);
                try { s_dal!.Engineer!.Read(id); } catch (Exception e) { break; }
            }
            while (true);
            Engineer engineer = new Engineer(
                id,
                names[i],
                $"{names[i]}@gmail.com",
                (DO.EngineerExperience)s_rand.Next(1, 6),
                (double)s_rand.Next(50, 200)
                );
            s_dal!.Engineer.Create(engineer);
        }
    }

    /// <summary>
    /// gets the list of tasks from the database and performs actions on each task in the list.
    /// </summary>
    private static void createDependencies()
    {
        List<DO.Task>? tasks = s_dal!.Task!.ReadAll().ToList();
        foreach (DO.Task task in tasks)
        {
            int taskId1 = task.Id;
            int taskId2 = tasks.Find(x => x.Alias?[0] + 1 == task.Alias?[0])?.Id ?? -1;
            int taskId3 = tasks.Find(x => x.Alias?[0] + 2 == task.Alias?[0])?.Id ?? -1;
            int taskId4 = tasks.Find(x => x.Alias?[0] + 3 == task.Alias?[0])?.Id ?? -1;
            if (taskId2 > -1)//if found a task 1 before me to be dependent on
            {
                DO.Dependency dependency = new Dependency(0, taskId1, taskId2);
                s_dal!.Dependency!.Create(dependency);
                CheckDependencies(dependency.Id);
            }
            if (taskId3 > -1)//if found a task 2 before me to be dependent on
            {
                DO.Dependency dependency = new Dependency(0, taskId1, taskId3);
                s_dal!.Dependency!.Create(dependency);
                CheckDependencies(dependency.Id);
            }
            if (taskId4 > -1)//if found a task 3 before me to be dependent on
            {
                DO.Dependency dependency = new Dependency(0, taskId1, taskId4);
                s_dal!.Dependency!.Create(dependency);
                CheckDependencies(dependency.Id);
            }
        }
    }

    private static void CheckDependencies(int id)
    {
        try
        {
            TopologicalSort(s_dal!.Dependency!.ReadAll()//send a graph to check that there are no circling dependencies
           .GroupBy(d => d.DependentTask).ToDictionary(
           group => group.Key,//dependent task
               group => group.Select(d => d.DependentTask).ToArray()//all dependencies for task
           ));
        }
        catch (Exception ex)
        {
            s_dal!.Dependency!.Delete(id);
        }
    }

    /// <summary>
    /// creates and initializes entity lists with the param interface variables
    /// </summary>
    /// <exception cref="Exception"></exception>
    public static void Do()
    {
        s_dal = Factory.Get; //stage 4
        createTasks();
        createEngineers();
        createDependencies();
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
            throw new DalCirclingDependenciesExeption(" has a cycle! No topological ordering exists.");
        }
    }
}
