namespace Dal;
internal static class DataSource
{
    /// <summary>
    /// This code defines a static internal class called Config, which contains constant and static variables referring to the next dependency and task numbers in the program.
    /// </summary>
    internal static class Config
    {
        internal const int startDependencyId = 1;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }

        internal const int startTaskId = 1;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }
    }
    internal static List<DO.Dependency> Dependencies { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.EngineerExperience> EngineerExperiences { get; } = new();
    internal static List<DO.Task> Tasks { get; } = new();
}
