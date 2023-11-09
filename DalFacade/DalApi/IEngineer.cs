namespace DO;
public interface IEngineer
{
    /// <summary>
    /// Creates new Engineer entity object in DAL
    /// </summary>
    /// <param name="item">new Engineer item to add</param>
    /// <returns>id of Engineer added</returns>
    int Create(Engineer item);
    /// <summary>
    /// Reads entity object by its ID 
    /// </summary>
    /// <param name="id">id of Engineer to read</param>
    /// <returns>Engineer with param id, if not found - null</returns>
    Engineer? Read(int id);
    /// <summary>
    /// Reads all entity objects
    /// </summary>
    /// <returns>list of all Engineers</returns>
    List<Engineer> ReadAll();
    /// <summary>
    /// Updates entity object
    /// </summary>
    /// <param name="item">Engineer with id to update and values to update</param>
    void Update(Engineer item);
    /// <summary>
    /// Deletes an object by its Id
    /// </summary>
    /// <param name="id">id of Engineer to delete</param>
    void Delete(int id);

}

