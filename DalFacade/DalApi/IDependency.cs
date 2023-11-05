namespace Do;

public interface IDependency
{
    int Create(IDependency item); //Creates new entity object in DAL
    IDependency? Read(int id); //Reads entity object by its ID 
    List<IDependency> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(IDependency item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id

}
