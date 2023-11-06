using Do;

namespace Dal
{
    internal class DependencyImplementation : IDependency
    {
        public int Create(IDependency item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IDependency? Read(int id)
        {
            throw new NotImplementedException();
        }

        public List<IDependency> ReadAll()
        {
            throw new NotImplementedException();
        }

        public void Update(IDependency item)
        {
            throw new NotImplementedException();
        }
    }
}
