namespace DO;

public interface ICrud<T> where T : class
{
    //all commants are for type dependency, need to be changed.
    /// <summary>
    /// Creates new Dependency entity object in DAL
    /// </summary>
    /// <param name="item">Dependency type</param>
    /// <returns>integer - new item id</returns>
    int Create(T item);
    /// <summary>
    /// Reads entity object by its ID 
    /// </summary>
    /// <param name="id">id of Dependency to read</param>
    /// <returns>Dependency object with param id, if not found - null</returns>
    T? Read(int id);
    /// <summary>
    /// Reads all entity objects
    /// </summary>
    /// <returns> list type Dependency of all dependencies</returns>
    List<T> ReadAll();
    /// <summary>
    /// Updates entity object
    /// </summary>
    /// <param name="item">new item - the item with id to update, and values to update</param>
    void Update(T item);
    /// <summary>
    /// Deletes an object by its Id
    /// </summary>
    /// <param name="id">id of dependency to delete</param>
    void Delete(int id);
}
