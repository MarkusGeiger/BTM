using BTM.Common;
using BTM.Configuration;
using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;

namespace BTM.TextFileAccess
{
  [Export(typeof(ITask))]
  [ExportMetadata("Name", "TextFileTask")]
  [ExportMetadata("Version", "1.0.1.2")]
  [JsonConverter(typeof(TaskJsonConverter))]
  public class TextFileTask : TaskBase, ITask
  {
    public override string Name => GetType().Name;

    public override Version Version => GetType().Assembly.GetName().Version;


    private void InitializeParameters()
    {
      Parameters = new ObservableCollection<ITaskProperty>
      {
        new TaskProperty<string>("FileName", String.Empty),
      };
    }

    public TextFileTask()
    {
      InitializeParameters();
    }
    
    public override ITask GetInstance()
    {
      return new TextFileTask();
    }

    protected override void DoWork()
    {
      throw new NotImplementedException();
    }
  }
}
