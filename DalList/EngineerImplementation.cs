using DO;

namespace Dal;

internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// Creates new Engineer entity object in DAL
    /// </summary>
    /// <param name="item">Engineer type</param>
    /// <returns>integer - new Engineer item id</returns>
    public int Create(Engineer item)
    {
        if (DataSource.Engineers.Any(e => e.Id == item.Id))
        {
            throw new DalAlreadyExistsException($"Object of type Engineer with ID {item.Id} exists.");
        }
        Engineer tempItem = new Engineer(item.Id, item.Name, item.Email, item.Level, item.Cost);
        DataSource.Engineers.Add(tempItem);
        return item.Id;
    }
    /// <summary>
    /// Deletes a Engineer object by its Id
    /// </summary>
    /// <param name="id">id of Engineer to delete</param>
    public void Delete(int id)
    {
        Engineer? deleteIt = (from e in DataSource.Engineers
                              where e.Id == id
                              select e).ToList().FirstOrDefault();

        if (deleteIt == null)
        {
            throw new DalDoesNotExistException($"Object of type Engineer with ID {id} does not exist.");
        }
        else
        {
            DataSource.Engineers.Remove(deleteIt);
        }
    }
    /// <summary>
    /// Reads Engineer entity object by its ID 
    /// </summary>
    /// <param name="id">id of Engineer to read</param>
    /// <returns>Engineer object with param id, if not found - null</returns>
    public Engineer? Read(int id)
    {
        return (from e in DataSource.Engineers
                where e.Id == id
                select e).ToList().FirstOrDefault();
    }
    /// <summary>
    /// Reads all entity objects by lambda function returning bool if wanted
    /// </summary>
    /// <param name="filter">not needed parameter of filtering list function, return true or false for object</param>
    /// <returns>return list filterd by filter, or full list</returns>
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers
               select item;
    }
    /// <summary>
    /// Updates Engineer entity object
    /// </summary>
    /// <param name="item">new Engineer item - the item with id to update, and values to update</param>
    public void Update(Engineer item)
    {
        Engineer? existingItem = (from e in DataSource.Engineers
                                  where e.Id == item.Id
                                  select e).ToList().FirstOrDefault();

        if (existingItem == null)
        {
            throw new DalDoesNotExistException($"Object of type Engineer with ID {item.Id} does not exist.");
        }
        else
        {
            DataSource.Engineers.Remove(existingItem);
            DataSource.Engineers.Add(item);
        }
    }
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers
            .FirstOrDefault(filter);
    }
}
