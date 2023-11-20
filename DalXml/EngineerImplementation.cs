namespace Dal;
using DO;


internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        List<Engineer> list = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        if (list.Any(e => e.Id == item.Id))
        {
            throw new DalAlreadyExistsException($"Object of type Engineer with ID {item.Id} exists.");
        }

        Engineer tempItem = new Engineer(item.Id, item.Name, item.Email, item.Level, item.Cost);
        list.Add(tempItem);
        return item.Id;
    }

    public void Delete(int id)
    {
        List<Engineer> list = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer? deleteIt = (from e in list
                              where e.Id == id
                              select e).ToList()[0];

        if (deleteIt == null)
        {
            throw new DalDoesNotExistException($"Object of type Engineer with ID {id} does not exist.");
        }
        else
        {
            list.Remove(deleteIt);
        }
        XMLTools.SaveListToXMLSerializer<Engineer>(list, "engineers");
    }

    public Engineer? Read(int id)
    {
        List<Engineer> list = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        return (from e in list
                where e.Id == id
                select e).ToList()[0];
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> list = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        return list
                  .FirstOrDefault(filter);
    }

    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> list = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        if (filter != null)
        {
            return from item in list
                   where filter(item)
                   select item;
        }
        return from item in list
               select item;
    }

    public void Update(Engineer item)
    {
        List<Engineer> list = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer? existingItem = (from e in list
                                  where e.Id == item.Id
                                  select e).ToList()[0];

        if (existingItem == null)
        {
            throw new DalDoesNotExistException($"Object of type Engineer with ID {item.Id} does not exist.");
        }
        else
        {
            list.Remove(existingItem);
            list.Add(item);
        }
        XMLTools.SaveListToXMLSerializer<Engineer>(list, "engineers");
    }
}
