using BlApi;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private readonly DO.IDal _dal = DO.Factory.Get;
    public BO.Milestone GetMilestone(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Milestone UpdateMilestone(int id)
    {
        throw new NotImplementedException();
    }
}
