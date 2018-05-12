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
      ComposeParts();
    }

    public static ExtensionManager Instance
    {
      get
      {
        return _Instance ?? (_Instance = new ExtensionManager());
      }
    }

    public IEnumerable<Lazy<ITask, ITaskMetaData>> AvailableParts { get; private set; }

    [ImportMany]
    private IEnumerable<Lazy<ITask, ITaskMetaData>> taskList;

    public void ComposeParts()
    {
      //An aggregate catalog that combines multiple catalogs  
      var catalog = new AggregateCatalog();
      //Adds all the parts found in the same assembly as the Program class  
      catalog.Catalogs.Add(new DirectoryCatalog(Path.Combine(Environment.CurrentDirectory, ADD_ON_DIRECTORY)));

      //Create the CompositionContainer with the parts in the catalog  
      _container = new CompositionContainer(catalog);

      //Fill the imports of this object  
      try
      {
        _container.ComposeParts(this);

        AvailableParts = taskList;
      }
      catch (CompositionException compositionException)
      {
        Console.WriteLine(compositionException.ToString());
      }
    }
  }
}
