using BTM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public static ExtensionManager Instance
    {
      get
      {
        return _Instance ?? (_Instance = new ExtensionManager());
      }
    }

    public IEnumerable<ITaskData> AvailableParts { get; private set; }

    [ImportMany]
    private IEnumerable<Lazy<ITask, ITaskData>> taskList;

    public void ComposeParts()
    {
      System.Threading.Thread.Sleep(5000);
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

        AvailableParts = taskList.Select(lazyPart => lazyPart.Metadata);
      }
      catch (CompositionException compositionException)
      {
        Console.WriteLine(compositionException.ToString());
      }
    }
  }
}
