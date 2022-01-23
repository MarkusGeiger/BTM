using BTM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;

namespace BTM.Extensibility
{
  public class ExtensionManager
  {
    private const string ADD_ON_DIRECTORY = "AddOns";
    private static ExtensionManager _Instance;
    private CompositionContainer _container;

    private ExtensionManager()
    {
    }

    public event Action CompositionCompletedEvent;

    private void RaiseCompositionCompletedEvent()
    {
      CompositionCompletedEvent?.Invoke();
    }

    public static ExtensionManager Instance
    {
      get
      {
        return _Instance ?? (_Instance = new ExtensionManager());
      }
    }

    public IEnumerable<Lazy<ITask, ITaskMetaData>> AvailableParts
    {
      get => _availableParts;
      private set
      {
        _availableParts = value;
        if(_availableParts != null) RaiseCompositionCompletedEvent();
      }
    }

    [ImportMany]
    private IEnumerable<Lazy<ITask, ITaskMetaData>> taskList;
    private IEnumerable<Lazy<ITask, ITaskMetaData>> _availableParts;

    public void ComposeParts()
    {
      //An aggregate catalog that combines multiple catalogs  
      var catalog = new AggregateCatalog();
      //Adds all the parts found in the same assembly as the Program class  
      var addOnStorage = Path.Combine(Environment.CurrentDirectory, ADD_ON_DIRECTORY);
      try
      {
        catalog.Catalogs.Add(new DirectoryCatalog(addOnStorage));
      }
      catch (DirectoryNotFoundException directoryNotFoundException)
      {
        Console.WriteLine(directoryNotFoundException.ToString());
        throw new ExtensibilityException($"AddOn storage directory {addOnStorage} not found.", directoryNotFoundException);
      }

      //Create the CompositionContainer with the parts in the catalog  
      _container = new CompositionContainer(catalog);

      //Fill the imports of this object  
      try
      {
        _container.ComposeParts(this);
        if (taskList != null && taskList.Any())
        {
          AvailableParts = taskList;
        }
        else
        {
          throw new ExtensibilityException($"No AddOns found in AddOn storage directory {addOnStorage}.");
        }
      }
      catch (CompositionException compositionException)
      {
        Console.WriteLine(compositionException.ToString());
        throw new ExtensibilityException($"No AddOns found in AddOn storage directory {addOnStorage}.", compositionException);
      }
    }
  }
}
