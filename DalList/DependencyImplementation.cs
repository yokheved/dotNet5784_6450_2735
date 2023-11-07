using DalApi;
using DO;

namespace Dal
{
    internal class DependencyImplementation : Dependency
    {
        public int Create(Dependency item)
        {
            int id = DataSource.Config.NextDependencyId;
            Dependency tempItem = new Dependency(id, item.DependentTask, item.DependsOnTask);
            DataSource.Dependencies.Add(tempItem);
           return id;
        }
        public  Dependency? Read(int id) 
        {
            return DataSource.Dependency.Find(ID => Item.id == ID);
        }
        public void Delete(int id)
        {
            Dependency deleteIt = DataSource.Dependency.Find(item => item.Id == id);

            if (deleteIt == null)
            {
                throw new Exception($"Object of type T with ID {id} does not exist.");
            }
            else
            { 
                    DataSource.Dependency.Remove(deleteIt);
            }
        }

        

       
        public void Update(Dependency item)
        {
            Dependency existingItem = DataSource.Dependencies.Find(dependency => dependency.Id == item.Id);

            if (existingItem == null)
            {
                throw new Exception($"Object of type T with ID {item.id} does not exist.");
            }
            else
            {
                DataSource.Dependencies.Remove(existingItem);
                DataSource.Dependencies.Add(item);
            }
        }

    }
}
