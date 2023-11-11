using DO;

namespace Dal;

internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// Creates new Dependency entity object in DAL
    /// </summary>
    /// <param name="item">Dependency type</param>
    /// <returns>integer - new item id</returns>
    public int Create(Dependency item)
    {
        if (DataSource.Dependencies.Contains(item))
        {
            throw new DalAlreadyExistsException($"Object of type Dependency with ID {item.Id} exists.");
        }
        int id = DataSource.Config.NextDependencyId;
        Dependency tempItem = new Dependency(id, item.DependentTask, item.DependsOnTask);
        DataSource.Dependencies.Add(tempItem);
        return id;
    }
    /// <summary>
    /// Reads entity object by its ID 
    /// </summary>
    /// <param name="id">id of Dependency to read</param>
    /// <returns>Dependency object with param id, if not found - null</returns>
    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.Find(Item => Item.Id == id);
    }
    /// <summary>
    /// Deletes an object by its Id
    /// </summary>
    /// <param name="id">id of dependency to delete</param>
    public void Delete(int id)
    {
        Dependency? deleteIt = DataSource.Dependencies.Find(item => item.Id == id);

        if (deleteIt == null)
        {
            throw new DalDoesNotExistException($"Object of type Dependency with ID {id} does not exist.");
        }
        else
        {
            DataSource.Dependencies.Remove(deleteIt);
        }
    }
    /// <summary>
    /// Updates entity object
    /// </summary>
    /// <param name="item">new item - the item with id to update, and values to update</param>
    public void Update(Dependency item)
    {
        Dependency? existingItem = DataSource.Dependencies.Find(dependency => dependency.Id == item.Id);

        if (existingItem == null)
        {
            throw new DalDoesNotExistException($"Object of type Dependency with ID {item.Id} does not exist.");
        }
        else
        {
            DataSource.Dependencies.Remove(existingItem);
            DataSource.Dependencies.Add(item);
        }
    }
    /// <summary>
    /// Reads all entity objects
    /// </summary>
    /// <returns> list type Dependency of all dependencies</returns>
    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }
}
