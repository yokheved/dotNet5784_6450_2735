namespace DO;
public interface IEngineer
{
    int Create(IEngineer item); //Creates new entity object in DAL
    IEngineer? Read(int id); //Reads entity object by its ID 
    List<IEngineer> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(IEngineer item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id

}

