namespace DO;

public interface IDependency
{
    /// <summary>
    /// Creates new Dependency entity object in DAL
    /// </summary>
    /// <param name="item">Dependency type</param>
    /// <returns>integer - new item id</returns>
    int Create(Dependency item);
    /// <summary>
    /// Reads entity object by its ID 
    /// </summary>
    /// <param name="id">id of Dependency to read</param>
    /// <returns>Dependency object with param id, if not found - null</returns>
    Dependency? Read(int id);
    /// <summary>
    /// Reads all entity objects
    /// </summary>
    /// <returns> list type Dependency of all dependencies</returns>
    List<Dependency> ReadAll();
    /// <summary>
    /// Updates entity object
    /// </summary>
    /// <param name="item">new item - the item with id to update, and values to update</param>
    void Update(Dependency item);
    /// <summary>
    /// Deletes an object by its Id
    /// </summary>
    /// <param name="id">id of dependency to delete</param>
    void Delete(int id);
}
