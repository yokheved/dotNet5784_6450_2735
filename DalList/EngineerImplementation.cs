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
        if (DataSource.Engineers.Contains(item))
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
        Engineer? deleteIt = DataSource.Engineers.Find(item => item.Id == id);

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
        return DataSource.Engineers.Find(Item => Item.Id == id);
    }
    /// <summary>
    /// Reads all Engineer entity objects
    /// </summary>
    /// <returns> list type Engineer of all Engineers</returns>
    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }
    /// <summary>
    /// Updates Engineer entity object
    /// </summary>
    /// <param name="item">new Engineer item - the item with id to update, and values to update</param>
    public void Update(Engineer item)
    {
        Engineer? existingItem = DataSource.Engineers.Find(Engineer => Engineer.Id == item.Id);

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
}
