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
  [ExportMetadata("Version", "1.0.0.2")]
  [JsonConverter(typeof(TaskJsonConverter))]
  public class TextFileTask : BindableBase, ITask
  {
    public ITask Previous { get; set; }
    public ITask Next { get; set; }
    public IEnumerable<ITaskProperty> Parameters { get; set; }
    public IEnumerable<object> InputValues { get; set; }
    public IEnumerable<object> OutputValues { get; set; }

    public string Name => GetType().Name;

    public Version Version => GetType().Assembly.GetName().Version;

    public bool IsFirst { get; set; }

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

    public IEnumerable<string> GetParameterList()
    {
      return Parameters != null ? Parameters.Select(param => param.Name) : Enumerable.Empty<string>();
    }

    public void SetParameter(string name, string value)
    {
      if (Parameters != null && Parameters.Any(param => param.Name == name))
      {
        Parameters.First(param => param.Name == name).Value = value;
      }
    }

    public ITask GetInstance()
    {
      return new TextFileTask();
    }

    public void Run()
    {
      throw new NotImplementedException();
    }
  }
}
