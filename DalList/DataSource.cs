namespace Dal;
internal static class DataSource
{
    internal static class Config///This code defines a static internal class called Config, which contains constant and static variables referring to the next dependency and task numbers in the program.
    {
        internal const int startDependencyId = 1;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }

        internal const int startTaskId = 1;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }
    }
    /// four static lists named Dependencies, Engineers, EngineerExperiences, and Tasks. They contain our data and allow read access to this data.
    internal static List<DO.Dependency> Dependencies { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.EngineerExperience> EngineerExperiences { get; } = new();
    internal static List<DO.Task> Tasks { get; } = new();
    //public static object Dependency { get; internal set; }
}
