﻿namespace Dal;
using DO;


internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// Creates new Engineer entity object in DAL
    /// </summary>
    /// <param name="item">Engineer type</param>
    /// <returns>integer - new Engineer item id</returns>
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
    /// <summary>
    /// Deletes a Engineer object by its Id
    /// </summary>
    /// <param name="id">id of Engineer to delete</param>
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
    /// <summary>
    /// Reads Engineer entity object by its ID 
    /// </summary>
    /// <param name="id">id of Engineer to read</param>
    /// <returns>Engineer object with param id, if not found - null</returns>
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
    /// <summary>
    /// Reads all entity objects by lambda function returning bool if wanted
    /// </summary>
    /// <param name="filter">not needed parameter of filtering list function, return true or false for object</param>
    /// <returns>return list filterd by filter, or full list</returns>
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
    /// <summary>
    /// Updates Engineer entity object
    /// </summary>
    /// <param name="item">new Engineer item - the item with id to update, and values to update</param>
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
