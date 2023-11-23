﻿namespace Dal;
using DO;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        try
        {
            // Load the existing dependencies from the XML file
            XElement dependenciesRoot = XMLTools.LoadListFromXMLElement("Dependencies");

            // Get the next ID for the new dependency
            int nextId = XMLTools.GetAndIncreaseNextId("DataConfig.xml", "NextDependencyId");

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
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the creation process
            Console.WriteLine("An error occurred while creating the item: " + ex.Message);
            return -1; // Return a negative value to indicate an error
        }
    }

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

    public Dependency Read(int id)
    {
        // Load the XML data from the "Dependency" element
        XElement rootElem = XMLTools.LoadListFromXMLElement("Dependency");

        // Iterate through each element in the XML
        foreach (XElement elem in rootElem.Elements())
        {
            // Parse the "Id" element value to an integer
            int dependencyId = int.Parse(elem.Element("Id")?.Value ?? "");

            // Check if the parsed id matches the provided id
            if (dependencyId == id)
            {
                // Create a new Dependency object and populate its properties
                Dependency dependency = new Dependency
                {
                    Id = int.Parse(elem.Element("Id").Value),
                    DependsOnTask = elem.Element("DependsOnTask").Value,
                    DependentTask = elem.Element("DependentTask").Value
                };

                // Return the created dependency object
                return dependency;
            }
        }

        // If no matching dependency is found, return null
        return null;
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement rootElem = XMLTools.LoadListFromXMLElement("Dependency");

        foreach (XElement elem in rootElem.Elements())
        {
            Dependency dependency = new Dependency
            {
                Id = int.Parse(elem.Element("Id").Value),
                DependsOnTask = elem.Element("DependsOnTask").Value,
                DependentTask = elem.Element(" DependentTask,").Value,
            };

            if (filter(dependency))
            {
                return dependency;
            }
        }

        return null;
    }

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement rootElem = XMLTools.LoadListFromXMLElement("Dependency");

        foreach (XElement elem in rootElem.Elements())
        {
            Dependency dependency = new Dependency
            {
                Id = int.Parse(elem.Element("Id").Value),
                DependsOnTask = elem.Element("DependsOnTask").Value,
                DependentTask = elem.Element(" DependentTask,").Value,

            };

            if (filter == null || filter(dependency))
            {
                yield return dependency;
            }
        }
    }

    public void Update(Dependency item)
    {

        XElement root = XMLTools.LoadListFromXMLElement("Dependencies");
        XElement dependentToUpdate = root.Elements("Dependency")
                                      .First(e => e.Element("Id")?.Value == item.Id.ToString());

        if (dependentToUpdate != null)
        {
            // Update the properties of the entity XElement with the new values
            dependentToUpdate.Element("Id")?.SetValue(item.Id);
            dependentToUpdate.Element(" DependentTask,")?.SetValue(item.DependentTask,);
            dependentToUpdate.Element("DependsOnTask")?.SetValue(item.DependsOnTask);

            XMLTools.SaveListToXMLElement(root, "Dependencies"); 


        }

    }
       
    
}
