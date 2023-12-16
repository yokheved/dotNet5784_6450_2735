namespace DO;

public interface ITask : ICrud<Task>
{
    public void Deconstruct(DO.Task? t, out int id, out string? discription, out string? alias, out bool? isMilestone,
        out DateTime createdAtDate, out DateTime? startDate, out DateTime? scheduledDate, out DateTime? forecastDate,
        out DateTime? deadlineDate, out DateTime? completeDate, out string? deliverables, out string? remarks, out int? engineerId,
        out int? complexityLevel);
}