using DO;

namespace Dal;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)///The function checks if there is already an engineer with the same ID in the list. If so, it throws an exception. Otherwise, it adds the engineer to the list and returns their ID.
    {
        if (DataSource.Engineers.Contains(item))
        {
            throw new Exception($"Object of type Engineer with ID {item.Id} exists.");
        }
        Engineer tempItem = new Engineer(item.Id, item.Name, item.Email, item.Level, item.Cost);
        DataSource.Engineers.Add(tempItem);
        return item.Id;
    }

    public void Delete(int id)///The function searches for the engineer with the given ID in the list and removes them if found. Otherwise, it throws an exception.
    {
        Engineer? deleteIt = DataSource.Engineers.Find(item => item.Id == id);

        if (deleteIt == null)
        {
            throw new Exception($"Object of type Engineer with ID {id} does not exist.");
        }
        else
        {
            DataSource.Engineers.Remove(deleteIt);
        }
    }

    public Engineer? Read(int id)/// The function searches for the engineer with the given ID in the list and returns them if found. Otherwise, it returns null.
    {
        return DataSource.Engineers.Find(Item => Item.Id == id);
    }

    public List<Engineer> ReadAll()///The function returns a list of all the engineers in the database
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)///The function updates the engineer in the list if it exists, and throws an exception if it doesn't
    {
        Engineer? existingItem = DataSource.Engineers.Find(Engineer => Engineer.Id == item.Id);

        if (existingItem == null)
        {
            throw new Exception($"Object of type Engineer with ID {item.Id} does not exist.");
        }
        else
        {
            DataSource.Engineers.Remove(existingItem);
            DataSource.Engineers.Add(item);
        }
    }
}
