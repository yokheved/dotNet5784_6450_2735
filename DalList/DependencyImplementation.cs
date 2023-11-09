using DO;

namespace Dal;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)///Creates a new dependency and returns its identifier
    {
        if (DataSource.Dependencies.Contains(item))
        {
            throw new Exception($"Object of type Dependency with ID {item.Id} exists.");
        }
        int id = DataSource.Config.NextDependencyId;
        Dependency tempItem = new Dependency(id, item.DependentTask, item.DependsOnTask);
        DataSource.Dependencies.Add(tempItem);
        return id;
    }
    public Dependency? Read(int id)///Reads the dependency with the given identifier
    {
        return DataSource.Dependencies.Find(Item => Item.Id == id);
    }
    public void Delete(int id)///Deletes the dependency with the given identifier
    {
        Dependency? deleteIt = DataSource.Dependencies.Find(item => item.Id == id);

        if (deleteIt == null)
        {
            throw new Exception($"Object of type Dependency with ID {id} does not exist.");
        }
        else
        {
            DataSource.Dependencies.Remove(deleteIt);
        }
    }




    public void Update(Dependency item)/// Updates the dependency with the new details
    {
        Dependency? existingItem = DataSource.Dependencies.Find(dependency => dependency.Id == item.Id);

        if (existingItem == null)
        {
            throw new Exception($"Object of type Dependency with ID {item.Id} does not exist.");
        }
        else
        {
            DataSource.Dependencies.Remove(existingItem);
            DataSource.Dependencies.Add(item);
        }
    }

    public List<Dependency> ReadAll()///Reads all dependencies and returns them in a list
    {
        return new List<Dependency>(DataSource.Dependencies);
    }
}
