using DO;

namespace Dal;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        Engineer tempItem = new Engineer(item.Id, item.Name, item.Email, item.Level, item.Cost);
        DataSource.Engineers.Add(tempItem);
        return item.Id;
    }

    public void Delete(int id)
    {
        Engineer? deleteIt = DataSource.Engineers.Find(item => item.Id == id);

        if (deleteIt == null)
        {
            throw new Exception($"Object of type T with ID {id} does not exist.");
        }
        else
        {
            DataSource.Engineers.Remove(deleteIt);
        }
    }

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.Find(Item => Item.Id == id);
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        Engineer? existingItem = DataSource.Engineers.Find(Engineer => Engineer.Id == item.Id);

        if (existingItem == null)
        {
            throw new Exception($"Object of type T with ID {item.Id} does not exist.");
        }
        else
        {
            DataSource.Engineers.Remove(existingItem);
            DataSource.Engineers.Add(item);
        }
    }
}
