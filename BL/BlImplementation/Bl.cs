using BlApi;

namespace BlImplementation;

internal class Bl : IBl
{
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public IMilestone Milestone => new MilestoneImplementation();

    public void Reset(string? entity = "")
    {
        if (entity == "dependency")
            DO.Factory.Get.Dependency!.Reset();
        else
            DO.Factory.Get.Reset();
    }
}
