namespace Dal;
using DO;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// Creates new Dependency entity object in DAL
    /// </summary>
    /// <param name="item">Dependency type</param>
    /// <returns>integer - new item id</returns>
    public int Create(Dependency item)
    {
        // Load the existing dependencies from the XML file
        XElement dependenciesRoot = XMLTools.LoadListFromXMLElement("Dependencies");

        // Get the next ID for the new dependency
        int nextId = Config.NextDependencyId;

        // Create a new XElement for the new dependency
        XElement newDependency = new XElement("Dependency",
            new XElement("Id", nextId),
            new XElement("DependentTask", item.DependentTask),
            new XElement("DependsOnTask", item.DependsOnTask)

        );

        // Add the new dependency to the dependencies root element
        dependenciesRoot.Add(newDependency);

        // Save the updated dependencies root element to the XML file
        XMLTools.SaveListToXMLElement(dependenciesRoot, "Dependencies");

        // Return the ID of the created item
        return nextId;
    }
    /// <summary>
    /// Deletes an object by its Id
    /// </summary>
    /// <param name="id">id of dependency to delete</param>
    public void Delete(int id)
    {
        string entity = "Dependencies";

        XElement rootElem = XMLTools.LoadListFromXMLElement(entity);
        XElement elementToDelete = rootElem.Elements().First(e => e.Element("Id")?.Value == id.ToString());

        if (elementToDelete != null)
        {
            elementToDelete.Remove();
            XMLTools.SaveListToXMLElement(rootElem, entity);
        }
    }
    /// <summary>
    /// Reads entity object by its ID 
    /// </summary>
    /// <param name="id">id of Dependency to read</param>
    /// <returns>Dependency object with param id, if not found - null</returns>
    public Dependency? Read(int id)
    {
        // Load the XML data from the "Dependency" element
        XElement rootElem = XMLTools.LoadListFromXMLElement("Dependencies");
        IEnumerable<XElement> elements = rootElem.Elements();
        // Iterate through each element in the XML
        foreach (XElement elem in elements)
        {
            // Parse the "Id" element value to an integer
            int dependencyId = int.Parse(elem.Element("Id")!.Value);

            // Check if the parsed id matches the provided id
            if (dependencyId == id)
            {
                // Create a new Dependency object and populate its properties
                string DependsOnTask = elem.Element("DependsOnTask")!.Value,
               DependentTask = elem.Element("DependentTask")!.Value;
                Dependency? dependency = new Dependency
                (
                    dependencyId,
                    int.Parse(DependentTask),
                    int.Parse(DependsOnTask)
                );

                // Return the created dependency object
                return dependency ?? throw new DalDoesNotExistException($"dependency with id {id} does not exist");
            }
        }

        // If no matching dependency is found, return null
        return null;
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement rootElem = XMLTools.LoadListFromXMLElement("Dependencies");

        foreach (XElement elem in rootElem.Elements())
        {
            string id = elem.Element("Id")!.Value,
               DependsOnTask = elem.Element("DependsOnTask")!.Value,
               DependentTask = elem.Element("DependentTask")!.Value;
            Dependency? dependency = new Dependency
            (
                int.Parse(id),
                int.Parse(DependentTask),
                int.Parse(DependsOnTask)
            );

            if (filter(dependency))
            {
                return dependency ?? throw new DalDoesNotExistException($"dependency does not exist");
            }
        }

        return null;
    }
    /// <summary>
    /// Reads all entity objects by lambda function returning bool if wanted
    /// </summary>
    /// <param name="filter">not needed parameter of filtering list function, return true or false for object</param>
    /// <returns>return list filterd by filter, or full list</returns>
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement rootElem = XMLTools.LoadListFromXMLElement("Dependencies");
        List<Dependency> dependencies = new List<Dependency>();

        foreach (XElement elem in rootElem.Elements())
        {
            string id = elem.Element("Id")!.Value,
                DependsOnTask = elem.Element("DependsOnTask")!.Value,
                DependentTask = elem.Element("DependentTask")!.Value;
            Dependency? dependency = new Dependency
            (
                int.Parse(id),
                int.Parse(DependentTask),
                int.Parse(DependsOnTask)
            );

            if (filter == null || filter(dependency))
            {
                dependencies.Add(dependency);
            }
        }
        return dependencies;
    }

    public void Update(Dependency item)
    {

        XElement root = XMLTools.LoadListFromXMLElement("Dependencies");
        XElement dependentToUpdate = root.Elements("Dependency")
                                      .First(e => e.Element("Id")?.Value == item.Id.ToString());

        if (dependentToUpdate != null)
        {
            // Update the properties of the entity XElement with the new values
            dependentToUpdate.Element("Id")!.SetValue(item.Id);
            dependentToUpdate.Element("DependentTask")!.SetValue(item.DependentTask);
            dependentToUpdate.Element("DependsOnTask")!.SetValue(item.DependsOnTask);

            XMLTools.SaveListToXMLElement(root, "Dependencies");
        }

    }

    public void Reset()
    {
        XMLTools.ResetFile("Dependencies");
    }
}
