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
        if (DataSource.Dependencies.Any(dep => dep.Id == item.Id)
)
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
        return (from d in DataSource.Dependencies
                where d.Id == id
                select d).ToList()[0];
    }
    /// <summary>
    /// Deletes an object by its Id
    /// </summary>
    /// <param name="id">id of dependency to delete</param>
    public void Delete(int id)
    {
        Dependency? deleteIt = (from d in DataSource.Dependencies
                                where d.Id == id
                                select d).ToList()[0];

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
        Dependency? existingItem = (from d in DataSource.Dependencies
                                    where d.Id == item.Id
                                    select d).ToList()[0];

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
    /// Reads all entity objects by lambda function returning bool if wanted
    /// </summary>
    /// <param name="filter">not needed parameter of filtering list function, return true or false for object</param>
    /// <returns>return list filterd by filter, or full list</returns>
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Dependencies
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependencies
               select item;
    }

}
