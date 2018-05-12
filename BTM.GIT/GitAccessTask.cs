using BTM.Common;
using BTM.Configuration;
using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace BTM.GIT
{
  [Export(typeof(ITask))]
  [ExportMetadata("Name", "GitAccessTask")]
  [ExportMetadata("Version", "1.0.0.1")]
  [JsonConverter(typeof(TaskJsonConverter))]
  public class GitAccessTask : BindableBase, ITask
  {
    public ITask Previous { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ITask Next { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IEnumerable<object> InputValues { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IEnumerable<object> OutputValues { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IEnumerable<ITaskProperty> Parameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public string Name => GetType().Name;

    public Version Version => GetType().Assembly.GetName().Version;

    public bool IsFirst { get; set; }

    public ITask GetInstance()
    {
      return new GitAccessTask();
    }

    public IEnumerable<string> GetParameterList()
    {
      throw new NotImplementedException();
    }

    public void Run()
    {
      throw new NotImplementedException();
    }

    public void SetParameter(string name, string value)
    {
      throw new NotImplementedException();
    }
  }
}
